namespace Game.General.Services
{
    using System;
    using System.Collections.Generic;
    using Effects;
    using static Effects.DiceType;


    public interface IEnemyProvider
    {
        CreatureConfig Current { get; }

        void Next();

        string Id { get; }
    }

    public class EnemyProvider : IEnemyProvider
    {
        public CreatureConfig Current => currentIndex > enemies.Count - 1 ? null : enemies[currentIndex];

        private List<CreatureConfig> enemies = new List<CreatureConfig>()
        {
            new CreatureConfig
            {
                BodyParts = new List<BodyPart>
                {
                    BodyPart.Head
                },
                Dices = new List<Dice>
                {
                    new(new List<DiceType>
                    {
                        One,
                        Two,
                        Three,
                        Four,
                        Five,
                        Six,
                    })
                },
                MaxHealth = 20
            }
        };

        private int currentIndex = -1;

        public void Next()
        {
            currentIndex++;
        }

        public string Id { get; } = Guid.NewGuid().ToString();
    }
}