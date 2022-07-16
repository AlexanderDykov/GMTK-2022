namespace Game.General.Services
{
    using System;
    using System.Collections.Generic;
    using Effects;
    using static Effects.DiceType;

    public interface IPlayerProvider
    {
        CreatureConfig PlayerConfig { get; }

        string Id { get; }
    }

    public class PlayerProvider : IPlayerProvider
    {
        public CreatureConfig PlayerConfig => new()
        {
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
            MaxHealth = 20
        };

        public string Id { get; } = Guid.NewGuid().ToString();
    }
}