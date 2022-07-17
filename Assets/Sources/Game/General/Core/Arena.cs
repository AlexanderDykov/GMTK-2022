namespace Game.General
{
    using System.Collections.Generic;
    using System.Linq;
    using Game.General.Effects;
    using UnityEngine;

    public class Arena
    {
        public Dictionary<string, Creature> Creatures = new Dictionary<string, Creature>();

        private readonly SpellBook _spellBook;

        public Arena(SpellBook spellBook)
        {
            _spellBook = spellBook;
        }

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
}
