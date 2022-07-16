namespace Game.General
{
    using System.Collections.Generic;
    using Effects;

    public class Dice
    {
        private readonly List<DiceType> _diceTypes;

        public Dice(List<DiceType> diceTypes)
        {
            _diceTypes = diceTypes;
        }

        public DiceType Random()
        {
            int rndIndex = UnityEngine.Random.Range(0, _diceTypes.Count);
            return _diceTypes[rndIndex];
        }
    }
}