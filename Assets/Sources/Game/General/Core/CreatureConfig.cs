namespace Game.General
{
    using System;
    using System.Collections.Generic;
    using Game.General.Effects;
    using UnityEngine;

    public interface IChooseMovesStrategy
    {
        Dictionary<Target, List<Move>> ChooseMoves(Creature self, Arena arena, Turn turn);
    }

    public class RandomChooseMovesStrategy : IChooseMovesStrategy
    {
        public Dictionary<Target, List<Move>> ChooseMoves(Creature self, Arena arena, Turn turn)
        {
            var result = new Dictionary<Target, List<Move>>();
            foreach (var currentDice in self.Config.Dices)
            {
                var randomDice = currentDice.Random();
                var target = self;
                var isTargetSelf = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
                if (!isTargetSelf)
                {
                    foreach (var creature in arena.Creatures.Values)
                    {
                        if (creature.Config.SpriteName == "player")
                        {
                            target = creature;
                            break;
                        }
                    }
                }
                var bodyPart = target.Config.BodyParts.RandomElement();
                if (isTargetSelf)
                {
                    Debug.LogError("Enemy defends his " + bodyPart + " with value = " + randomDice);
                }
                else
                {
                    Debug.LogError("Enemy attack players " + bodyPart + " with value = " + randomDice);
                }
                result.Add(new Target()
                {
                    Id = target.Id,
                    BodyPart = bodyPart
                }, new List<Move>()
                {
                    new()
                    {
                        SourceId = self.Id,
                        DiceTypes = new List<DiceType>
                        {
                            randomDice
                        }
                    }
                });
            }
            return result;
        }
    }

    public class CreatureConfig
    {
        public string SpriteName;

        public List<BodyPart> BodyParts;

        public List<Dice> Dices;

        public int MaxHealth;

        public IChooseMovesStrategy ChooseMovesStrategy;
    }

    public class Creature
    {
        private int _currentHealth;

        public readonly CreatureConfig Config;

        public string Id { get; private set; }

        public Creature(CreatureConfig config, string id)
        {
            _currentHealth = config.MaxHealth;
            Config = config;
            Id = id;
        }

        public bool IsDead => _currentHealth <= 0;
        public int CurrentHealth => _currentHealth;

        public event Action<int> CurrentHealthChanged;

        public void ApplyDamage(int damage)
        {
            _currentHealth -= damage;
            Debug.LogError("Creature " + Id + " received = " + damage + " and health = " + _currentHealth);
            _currentHealth = Math.Max(0, _currentHealth);
            CurrentHealthChanged?.Invoke(_currentHealth);
        }
    }
}
