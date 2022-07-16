namespace Game.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Effects;
    using UnityEngine;

    public class Arena
    {
        public Dictionary<string, Creature> Creatures = new Dictionary<string, Creature>();

        private SpellBook _spellBook = new SpellBook();

        public void ApplyTurn(Turn turn)
        {
            foreach (var targetAssignedMoves in turn.AssignedMoves)
            {
                var target = targetAssignedMoves.Key;
                var creature = Creatures[target.Id];
                var attackRecord = new AttackRecord();
                var effects = new Dictionary<EffectOrder, List<Effect>>();
                var allAssignedMoves = targetAssignedMoves.Value;
                var enumerable = allAssignedMoves.Select(move => _spellBook.Find(move)).ToList();
                foreach (var effect in enumerable)
                {
                    if (!effects.ContainsKey(effect.Order))
                    {
                        effects[effect.Order] = new List<Effect>();
                    }

                    effects[effect.Order].Add(effect);
                }

                foreach (var effect in effects.Values.SelectMany(effectList => effectList))
                {
                    effect.Execute(target, allAssignedMoves, attackRecord, effects);
                }

                var damage = attackRecord.Calculate();
                if (damage > 0)
                {
                    creature.ApplyDamage(damage);
                }
            }

            var notDeadList = new Dictionary<string, Creature>();
            foreach (var creature in Creatures)
            {
                if (!creature.Value.IsDead)
                {
                    notDeadList.Add(creature.Key, creature.Value);
                }
            }

            Creatures = notDeadList;
            if (Creatures.Count == 0)
            {
                Debug.LogError("End");
            }
        }
    }

    public class AttackRecord
    {
        public int Damage;

        public int Resist;

        public int Calculate()
        {
            if (Resist >= Damage)
                return 0;

            var calculate = Damage - Resist;
            return calculate;
        }
    }

    public abstract class Effect
    {
        public virtual EffectOrder Order => EffectOrder.Default;

        public Move Move { get; }
        protected Effect(Move move)
        {
            Move = move;
        }

        public void Execute(Target target,
                            List<Move> allAssignedMoves,
                            AttackRecord attackRec,
                            Dictionary<EffectOrder, List<Effect>> effects)
        {
            if (target.Id == Move.SourceId)
            {
                ResistEffect(target, allAssignedMoves, attackRec, effects);
            }
            else
            {
                DamageEffect(target, allAssignedMoves, attackRec, effects);
            }
        }

        public int GetBasicPower()
        {
            return Move.DiceTypes.Sum(x => x.GetValue());
        }

        protected virtual void DamageEffect(Target target,
                                            List<Move> allAssignedMoves,
                                            AttackRecord attackRec,
                                            Dictionary<EffectOrder, List<Effect>> effects)
        {
            attackRec.Damage += GetBasicPower();
        }

        protected virtual void ResistEffect(Target target,
                                            List<Move> allAssignedMoves,
                                            AttackRecord attackRec,
                                            Dictionary<EffectOrder, List<Effect>> effects)
        {
            attackRec.Resist += GetBasicPower();
        }
    }

    public class DefaultEffect : Effect
    {
        public DefaultEffect(Move move) : base(move)
        {
        }
    }

    public class OneOneComboEffect : Effect
    {
        public OneOneComboEffect(Move move) : base(move)
        {
        }

        protected override void DamageEffect(Target target,
                                             List<Move> allAssignedMoves,
                                             AttackRecord attackRec,
                                             Dictionary<EffectOrder, List<Effect>> effects)
        {
            base.DamageEffect(target, allAssignedMoves, attackRec, effects);
            attackRec.Damage += 10;
        }

        protected override void ResistEffect(Target target,
                                             List<Move> allAssignedMoves,
                                             AttackRecord attackRec,
                                             Dictionary<EffectOrder, List<Effect>> effects)
        {
            base.ResistEffect(target, allAssignedMoves, attackRec, effects);
            int maxAttackDice = 0;
            foreach (var move in allAssignedMoves)
            {
                if (target.Id != move.SourceId)
                {
                   maxAttackDice = Math.Max(maxAttackDice, move.DiceTypes.Max(x => x.GetValue()));
                }
            }
            attackRec.Resist += maxAttackDice;
        }
    }

    public class TwoTwoComboEffect : Effect
    {
        public TwoTwoComboEffect(Move move) : base(move)
        {
        }

        protected override void DamageEffect(Target target,
                                             List<Move> allAssignedMoves,
                                             AttackRecord attackRec,
                                             Dictionary<EffectOrder, List<Effect>> effects)
        {
            attackRec.Damage += GetBasicPower() * 2;
        }

        protected override void ResistEffect(Target target,
                                             List<Move> allAssignedMoves,
                                             AttackRecord attackRec,
                                             Dictionary<EffectOrder, List<Effect>> effects)
        {
            attackRec.Resist += GetBasicPower() * 2;
        }
    }

    public class ThreeThreeComboEffect : Effect
    {

        public override EffectOrder Order => EffectOrder.Pre;

        public ThreeThreeComboEffect(Move move) : base(move)
        {
        }

        protected override void DamageEffect(Target target,
                                             List<Move> allAssignedMoves,
                                             AttackRecord attackRec,
                                             Dictionary<EffectOrder, List<Effect>> effects)
        {
            base.DamageEffect(target, allAssignedMoves, attackRec, effects);
            var lines = effects.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            if (effects.ContainsKey(EffectOrder.Default))
            {
                var defaultEffects = effects[EffectOrder.Default];
                for (int i = 0; i < defaultEffects.Count; i++)
                {
                    if (target.Id == defaultEffects[i].Move.SourceId)
                    {
                        defaultEffects[i] = new DefaultEffect(new Move
                        {
                            SourceId = defaultEffects[i].Move.SourceId,
                            DiceTypes = new List<DiceType>()
                        });
                    }
                }
            }
        }

        protected override void ResistEffect(Target target,
                                             List<Move> allAssignedMoves,
                                             AttackRecord attackRec,
                                             Dictionary<EffectOrder, List<Effect>> effects)
        {
            base.ResistEffect(target, allAssignedMoves, attackRec, effects);
            if (effects.ContainsKey(EffectOrder.Default))
            {
                var defaultEffects = effects[EffectOrder.Default];
                for (int i = 0; i < defaultEffects.Count; i++)
                {
                    var move = defaultEffects[i].Move;
                    if (target.Id != move.SourceId)
                    {
                        var diceNumPerTypes = new Dictionary<DiceType, int>();
                        foreach (var diceType in move.DiceTypes)
                        {
                            var num = 0;
                            diceNumPerTypes.TryGetValue(diceType, out num);
                            diceNumPerTypes[diceType] = num + 1;
                        }
                        foreach (var diceNumPerType in diceNumPerTypes)
                        {
                            if (diceNumPerType.Value == 2)
                            {
                                defaultEffects[i] = new DefaultEffect(new Move
                                {
                                    SourceId = move.SourceId,
                                    DiceTypes = move.DiceTypes.Where(x => x != diceNumPerType.Key).ToList()
                                });
                                return;
                            }
                        }
                    }
                }
            }
        }
    }

    public class FourFourComboEffect : Effect
    {
        public FourFourComboEffect(Move move) : base(move)
        {
        }

        protected override void DamageEffect(Target target,
                                             List<Move> allAssignedMoves,
                                             AttackRecord attackRec,
                                             Dictionary<EffectOrder, List<Effect>> effects)
        {
            base.DamageEffect(target, allAssignedMoves, attackRec, effects);
            foreach (var move in allAssignedMoves)
            {
                if (target.Id == move.SourceId)
                {
                    var weakestDefenceDice = move.DiceTypes.Min(x => x.GetValue());
                    attackRec.Damage += weakestDefenceDice;
                    attackRec.Resist -= weakestDefenceDice;
                    return;
                }
            }
        }
    }

    public class Spell
    {
        public string AttackDescription;
        public string DefenceDescription;
        public Func<Move, Effect> EffectMaker;
    }

    public class SpellBook
    {
        private Dictionary<List<DiceType>, Spell> _spells = new();

        public SpellBook()
        {
            _spells.Add(new List<DiceType>
            {
                DiceType.One,
                DiceType.One,
            }, new Spell
            {
                AttackDescription = "+10 to damage",
                DefenceDescription = "Block strongest attack dice",
                EffectMaker = move => new OneOneComboEffect(move)
            });
            _spells.Add(new List<DiceType>
            {
                DiceType.Two,
                DiceType.Two,
            }, new Spell
            {
                AttackDescription = "Double damage",
                DefenceDescription = "Double resist",
                EffectMaker = move => new TwoTwoComboEffect(move)
            });
            _spells.Add(new List<DiceType>
            {
                DiceType.Three,
                DiceType.Three,
            }, new Spell
            {
                AttackDescription = "Ignore all opponent resist",
                DefenceDescription = "Block first double dices damage and their effect (if any)",
                EffectMaker = move => new ThreeThreeComboEffect(move)
            });
            _spells.Add(new List<DiceType>
            {
                DiceType.Four,
                DiceType.Four,
            }, new Spell
            {
                AttackDescription = "+X to Damage where X is the weakest defence dice",
                DefenceDescription = "Nothing special... for now",
                EffectMaker = move => new FourFourComboEffect(move)
            });
        }

        public Effect Find(Move move)
        {
            return _spells
                .Where(x => x.Key.Count == move.DiceTypes.Count && x.Key.All(move.DiceTypes.Contains))
                .Select(x => x.Value.EffectMaker(move))
                .FirstOrDefault() ?? new DefaultEffect(move);
        }
    }

    public enum EffectOrder
    {
        Initialize,
        Pre,
        Default,
        Post,
        Finalize
    }
}
