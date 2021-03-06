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
        void Clear();
        void Restart();
    }

    public class EnemyProvider : IEnemyProvider
    {
        public CreatureConfig Current => currentIndex > enemies.Count - 1 ? null : enemies[currentIndex];

        private static Dice MakeSimpleDice()
        {
            return new Dice(new List<DiceType>
            {
                One,
                Two,
                Three,
                Four,
                Five,
                Six,
            });
        }

        private static CreatureConfig MakeRat()
        {
            return new CreatureConfig
            {
                Name = "rat",
                SpriteName = "Rat",
                BodyParts = new List<BodyPart>
                {
                    BodyPart.Head
                },
                Dices = new List<Dice>
                {
                    MakeSimpleDice()
                },
                MaxHealth = 20,
                ChooseMovesStrategy = new DuxStrategy()
            };
        }

        private static CreatureConfig MakeGoblin()
        {
            return new CreatureConfig
            {
                Name = "goblin",
                SpriteName = "Goblin",
                BodyParts = new List<BodyPart>
                {
                    BodyPart.Head,
                    BodyPart.Body,
                },
                Dices = new List<Dice>
                {
                    MakeSimpleDice(),
                    MakeSimpleDice()
                },
                MaxHealth = 36,
                ChooseMovesStrategy = new GoblinStrategy()
            };
        }

        // private static CreatureConfig MakeOrk()
        // {
        //     return new CreatureConfig
        //     {
        //         Name = "ork",
        //         SpriteName = "Ork",
        //         BodyParts = new List<BodyPart>
        //         {
        //             BodyPart.Head,
        //             BodyPart.Body,
        //             BodyPart.LeftHand,
        //             BodyPart.RightHand,
        //         },
        //         Dices = new List<Dice>
        //         {
        //             MakeSimpleDice(),
        //             MakeSimpleDice()
        //         },
        //         MaxHealth = 54,
        //         ChooseMovesStrategy = new RandomChooseMovesStrategy()
        //     };
        // }

        private static CreatureConfig MakeTroll()
        {
            return new CreatureConfig
            {
                Name = "troll",
                SpriteName = "Troll",
                BodyParts = new List<BodyPart>
                {
                    BodyPart.Head,
                    BodyPart.Body,
                    BodyPart.LeftHand,
                    BodyPart.RightHand,
                    BodyPart.LeftLeg,
                },
                Dices = new List<Dice>
                {
                    MakeSimpleDice(),
                    MakeSimpleDice(),
                    MakeSimpleDice()
                },
                MaxHealth = 76,
                ChooseMovesStrategy = new RandomChooseMovesStrategy()
            };
        }

        private static CreatureConfig MakeHydra()
        {
            return new CreatureConfig
            {
                Name = "hydra",
                SpriteName = "Hydra",
                BodyParts = new List<BodyPart>
                {
                    BodyPart.Head,
                    BodyPart.Head1,
                    BodyPart.Body,
                    BodyPart.LeftLeg,
                    BodyPart.RightLeg
                },
                Dices = new List<Dice>
                {
                    MakeSimpleDice(),
                    MakeSimpleDice(),
                    MakeSimpleDice(),
                    MakeSimpleDice(),
                    MakeSimpleDice()
                },
                MaxHealth = 120,
                ChooseMovesStrategy = new RandomChooseMovesStrategy()
            };
        }

        private List<CreatureConfig> enemies = new List<CreatureConfig>()
        {
            MakeRat(),
            MakeGoblin(),
            // MakeOrk(),
            MakeTroll(),
            MakeHydra()
        };

        private int currentIndex = 0;

        public void Next()
        {
            currentIndex++;
        }

        public string Id { get; } = Guid.NewGuid().ToString();
        public void Clear()
        {
            enemies.Clear();
        }

        public void Restart()
        {
            currentIndex = 0;
        }
    }
}
