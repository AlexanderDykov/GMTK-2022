using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.General.Effects
{
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

    public enum EffectOrder
    {
        Initialize,
        Pre,
        Default,
        Post,
        Finalize
    }

    public abstract class Effect
    {
        public virtual EffectOrder Order => EffectOrder.Default;
        public virtual SpellType SpellType => SpellType.None;

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
        public override SpellType SpellType => SpellType.Water;

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
        public override SpellType SpellType => SpellType.Fire;
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
        public override SpellType SpellType => SpellType.Air;
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
        public override SpellType SpellType => SpellType.Ground;
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
}