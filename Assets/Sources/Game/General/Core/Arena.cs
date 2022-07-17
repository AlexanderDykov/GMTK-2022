namespace Game.General
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Game.General.Effects;
    using Services;
    using UnityEngine;
    using Views.BodyParts;

    public class Arena
    {
        public Dictionary<string, Creature> Creatures = new Dictionary<string, Creature>();

        public readonly SpellBook SpellBook;

        private ISpellVisualizerService _spellVisualizerService;

        private ISoundService _soundService;

        public Arena(SpellBook spellBook, ISpellVisualizerService spellVisualizerService, ISoundService soundService)
        {
            SpellBook = spellBook;
            _soundService = soundService;
            _spellVisualizerService = spellVisualizerService;
        }

        public async UniTask ApplyTurn(Turn turn)
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
                    var bodyPartView = Object.FindObjectsOfType<BodyPartView>()
                        .First(x => x.Id == target.Id && x.BodyPart == target.BodyPart);


                    if (effect.SpellType != SpellType.None)
                    {
                        // var transformPosition =new Vector2(bodyPartView.transform.position.x, bodyPartView.transform.position.y);
                        await _spellVisualizerService.Show(effect, target.Id != effect.Move.SourceId,
                            bodyPartView.shieldParent.position);
                    }

                    bodyPartView.PlayAttack();

                    effect.Execute(target, moves, attackRecord, effects);
                }

                _soundService.Play(SFX.Attack);
                await UniTask.Delay(200);

                var damage = attackRecord.Calculate();
                if (damage > 0)
                {
                    _soundService.Play(SFX.GetDamage);
                    creature.ApplyDamage(damage);
                }

                await UniTask.Delay(1000);
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