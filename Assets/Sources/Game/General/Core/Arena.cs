namespace Game.General
{
    using System.Collections.Generic;
    using System.Linq;
    using Game.General.Effects;
    using Services;
    using UnityEngine;
    using Views.BodyParts;

    public class Arena
    {
        public Dictionary<string, Creature> Creatures = new Dictionary<string, Creature>();

        public readonly SpellBook SpellBook;

        private ISpellVisualizerService _spellVisualizerService;

        public Arena(SpellBook spellBook, ISpellVisualizerService spellVisualizerService)
        {
            SpellBook = spellBook;
            _spellVisualizerService = spellVisualizerService;
        }

        public void ApplyTurn(Turn turn)
        {
            foreach (var movesPerTarget in turn.AssignedMoves)
            {
                var target = movesPerTarget.Key;
                var creature = Creatures[target.Id];
                var attackRecord = new AttackRecord();
                var effects = new Dictionary<EffectOrder, List<Effect>>();
                var moves = movesPerTarget.Value;
                var enumerable = moves.Select(move =>
                {
                    var effect = SpellBook.Find(move);
                    return effect;
                }).ToList();
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
                    if (effect.SpellType != SpellType.None)
                    {
                        var bodyPartView = Object.FindObjectsOfType<BodyPartView>()
                            .First(x => x.Id == target.Id && x.BodyPart == target.BodyPart);

                        _spellVisualizerService.Show(effect, target.Id != effect.Move.SourceId, bodyPartView.transform.position);
                    }

                    effect.Execute(target, moves, attackRecord, effects);
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
        }
    }
}
