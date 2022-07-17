namespace Game.General
{
    using Game.General.Effects;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using UnityEngine;

    public interface IChooseMovesStrategy
    {
        Dictionary<Target, List<Move>> ChooseMoves(Creature self, Arena arena, Turn turn);

        protected static Creature FindPlayer(Arena arena)
        {
            foreach (var creature in arena.Creatures.Values)
            {
                if (creature.Config.SpriteName == "player")
                {
                    return creature;
                }
            }
            return null;
        }

        protected static bool MaybeTrue(int probabilityPercents)
        {
            return UnityEngine.Random.Range(0f, 1f) <= (probabilityPercents / 100.0f);
        }

        protected static List<DiceType> RollDices(List<Dice> dices, int numOfDicesToRoll)
        {
            List<DiceType> rolls = new List<DiceType>();
            var safeNumOfDicesToRoll = Math.Min(dices.Count, numOfDicesToRoll);
            for (int i = 0; i < safeNumOfDicesToRoll; ++i)
            {
                rolls.Add(dices[i].Random());
            }
            return rolls;
        }

        protected static void LogMove(bool isDefenceMove,
                                      string name,
                                      Target target,
                                      List<DiceType> diceTypes)
        {
            string diceValues = "";
            foreach (var diceType in diceTypes)
            {
                diceValues += diceType + ":";
            }
            if (isDefenceMove)
            {
                Debug.LogError(name + " defends his " + target.BodyPart + " with value = " + diceValues);
            }
            else
            {
                Debug.LogError(name + " attack players " + target.BodyPart + " with value = " + diceValues);
            }
        }
    }

    public class RandomChooseMovesStrategy : IChooseMovesStrategy
    {
        public Dictionary<Target, List<Move>> ChooseMoves(Creature self, Arena arena, Turn turn)
        {
            var result = new Dictionary<Target, List<Move>>();
            foreach (var currentDice in self.Config.Dices)
            {
                var targetCreature = self;
                var isDefenceMove = IChooseMovesStrategy.MaybeTrue(50);
                if (!isDefenceMove)
                {
                    targetCreature = IChooseMovesStrategy.FindPlayer(arena);
                }
                var target = new Target()
                {
                    Id = targetCreature.Id,
                    BodyPart = targetCreature.Config.BodyParts.RandomElement()
                };
                var diceTypes = new List<DiceType>
                {
                    currentDice.Random()
                };
                IChooseMovesStrategy.LogMove(isDefenceMove,
                                             self.Config.SpriteName,
                                             target,
                                             diceTypes);
                var moves = new List<Move>()
                {
                    new()
                    {
                        SourceId = self.Id,
                        DiceTypes = diceTypes
                    }
                };
                result.Add(target, moves);
            }
            return result;
        }
    }

    public class DuxStrategy : IChooseMovesStrategy
    {
        public Dictionary<Target, List<Move>> ChooseMoves(Creature self, Arena arena, Turn turn)
        {
            var result = new Dictionary<Target, List<Move>>();
            var anyDice = self.Config.Dices.Last();
            if (anyDice != null)
            {
                bool isDefenceMove = false;
                if (self.GetHealthPercents() < 40)
                {
                    isDefenceMove = IChooseMovesStrategy.MaybeTrue(75);
                }
                else
                {
                    isDefenceMove = IChooseMovesStrategy.MaybeTrue(50);
                }
                var targetCreature = self;
                if (!isDefenceMove)
                {
                    targetCreature = IChooseMovesStrategy.FindPlayer(arena);
                }
                var target = new Target()
                {
                    Id = targetCreature.Id,
                    BodyPart = targetCreature.Config.BodyParts.RandomElement()
                };
                var diceTypes = new List<DiceType>
                {
                    anyDice.Random()
                };
                IChooseMovesStrategy.LogMove(isDefenceMove,
                                             self.Config.SpriteName,
                                             target,
                                             diceTypes);
                var moves = new List<Move>()
                {
                    new()
                    {
                        SourceId = self.Id,
                        DiceTypes = diceTypes
                    }
                };
                result.Add(target, moves);
            }
            return result;
        }
    }

    public class GoblinStrategy : IChooseMovesStrategy
    {
        public Dictionary<Target, List<Move>> ChooseMoves(Creature self, Arena arena, Turn turn)
        {
            var result = new Dictionary<Target, List<Move>>();
            var maxNumOfDices = 2;
            var rolls = IChooseMovesStrategy.RollDices(self.Config.Dices, maxNumOfDices);
            var player = IChooseMovesStrategy.FindPlayer(arena);
            var move = new Move
            {
                SourceId = self.Id,
                DiceTypes = rolls
            };
            var effect = arena.SpellBook.Find(move);
            var isCombo = (effect is DefaultEffect is false);
            if (isCombo)
            {
                var target = new Target
                {
                    Id = player.Id,
                    BodyPart = player.Config.BodyParts.RandomElement()
                };
                IChooseMovesStrategy.LogMove(false,
                                             self.Config.SpriteName,
                                             target,
                                             move.DiceTypes);
                result.Add(target, new List<Move>{ move });
            }
            else
            {
                var numOfDicesToAttack = 1;
                if ((self.GetHealthPercents() < 40) && IChooseMovesStrategy.MaybeTrue(50))
                {
                    numOfDicesToAttack += 1;
                }
                var attackDices = new List<DiceType>();
                var defenceDices = new List<DiceType>();
                for (int i = 0; i < rolls.Count; ++i)
                {
                    if (i < numOfDicesToAttack)
                    {
                        attackDices.Add(rolls[i]);
                    }
                    else
                    {
                        defenceDices.Add(rolls[i]);
                    }
                }
                if (attackDices.Count != 0)
                {
                    var target = new Target
                    {
                        Id = player.Id,
                        BodyPart = player.Config.BodyParts.RandomElement()
                    };
                    IChooseMovesStrategy.LogMove(false,
                                                 self.Config.SpriteName,
                                                 target,
                                                 attackDices);
                    result.Add(target, new List<Move>
                    {
                        new()
                        {
                            SourceId = self.Id,
                            DiceTypes = attackDices
                        }
                    });
                }
                if (defenceDices.Count != 0)
                {
                    var target = new Target
                    {
                        Id = self.Id,
                        BodyPart = self.Config.BodyParts.RandomElement()
                    };
                    IChooseMovesStrategy.LogMove(true,
                                                 self.Config.SpriteName,
                                                 target,
                                                 defenceDices);
                    result.Add(target, new List<Move>
                    {
                        new()
                        {
                            SourceId = self.Id,
                            DiceTypes = defenceDices
                        }
                    });
                }
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

        public int GetHealthPercents()
        {
            return (int)(((float)_currentHealth / Config.MaxHealth) * 100.0f);
        }
    }
}
