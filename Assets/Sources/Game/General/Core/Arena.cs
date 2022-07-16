namespace Game.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Effects;

    public class Arena
    {
        public Dictionary<string, Creature> Creatures = new Dictionary<string, Creature>();

        private SpellBook _spellBook;

        public void ApplyTurn(Turn turn)
        {
            foreach (var turnAssignedMove in turn.AssignedMoves)
            {
                var target = turnAssignedMove.Key;
                var creature = Creatures[target.Id];
                var attackRecord = new AttackRecord();
                var effects = new Dictionary<EffectOrder, List<Effect>>();
                
                foreach (var effect in turnAssignedMove.Value.Select(move => _spellBook.Find(move)))
                {
                    effects[effect.Order].Add(effect);
                }

                foreach (var effect in effects.Values.SelectMany(effectList => effectList))
                {
                    effect.Execute(target, attackRecord, effects);
                }

                //Item1 to class
                int damage = attackRecord.Calculate();
                creature.ApplyDamage(damage);
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
        }
    }

    public class AttackRecord
    {
        public int Damage;

        public int Resist;

        public int Calculate()
        {
            var calculate = Damage - Resist;
            return calculate;
        }
    }

    public abstract class Effect
    {
        public virtual EffectOrder Order => EffectOrder.Default;

        protected Move _move;

        protected Effect(Move move)
        {
            _move = move;
        }

        public abstract void Execute(Target target, AttackRecord attackRec,
            Dictionary<EffectOrder, List<Effect>> effects);
    }

    public class FireBallEffect : Effect
    {
        public FireBallEffect(Move move) : base(move)
        {
        }

        public override void Execute(Target target, AttackRecord attackRec,
            Dictionary<EffectOrder, List<Effect>> effects)
        {
            attackRec.Damage += _move.DiceTypes.Sum(x => x.GetValue()) + 2;
        }
    }

    public class DefaultEffect : Effect
    {
        public DefaultEffect(Move move) : base(move)
        {
        }

        public override void Execute(Target target, AttackRecord attackRec,
            Dictionary<EffectOrder, List<Effect>> effects)
        {
            attackRec.Damage += _move.DiceTypes.Sum(x => x.GetValue());
        }
    }

    public class SpellBook
    {
        private Dictionary<List<DiceType>, Func<Move, Effect>> _spells = new();

        public SpellBook()
        {
            _spells.Add(new List<DiceType>
            {
                DiceType.Two,
                DiceType.Fire,
            }, move => new FireBallEffect(move));
        }

        public Effect Find(Move move)
        {
            return _spells
                .Where(x => x.Key.Count == move.DiceTypes.Count && x.Key.All(move.DiceTypes.Contains))
                .Select(x => x.Value(move))
                .FirstOrDefault() ?? new DefaultEffect(move);
        }
    }

    public enum EffectOrder
    {
        Default
    }


    //spellbook Dict<List<DiceType>, EffectType>

    //EffectType -> Effect

    //
}