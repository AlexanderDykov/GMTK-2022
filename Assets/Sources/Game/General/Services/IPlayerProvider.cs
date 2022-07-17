namespace Game.General.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Effects;
    using Game.General.Views.BodyParts;
    using static Effects.DiceType;

    public interface IPlayerProvider
    {
        CreatureConfig PlayerConfig { get; }

        string Id { get; }
    }

    public class PlayerChooseMovesStrategy : IChooseMovesStrategy
    {
        public Dictionary<Target, List<Move>> ChooseMoves(Creature self, Arena arena, Turn turn)
        {
            var allBodyParts = UnityEngine.Object.FindObjectsOfType<BodyPartView>();
            return allBodyParts
                .Select(bodyPart => bodyPart.CreateAssignedMove())
                .Where(x => x.Item2.Count > 0)
                .ToDictionary(valueTuple => valueTuple.Item1, valueTuple => valueTuple.Item2);
        }
    }

    public class PlayerProvider : IPlayerProvider
    {
        public CreatureConfig PlayerConfig => new()
        {
            Name = "player",
            SpriteName = "Player",
            BodyParts = new List<BodyPart>
            {
                BodyPart.Head,
                BodyPart.Body
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
                }),
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
            MaxHealth = 20,
            ChooseMovesStrategy = new PlayerChooseMovesStrategy()
        };

        public string Id { get; } = Guid.NewGuid().ToString();
    }
}
