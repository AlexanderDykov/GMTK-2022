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
            return _diceTypes.RandomElement();
        }
    }

    public static class ListExtensions
    {
        public static T RandomElement<T>(this List<T> elements)
        {
            int rndIndex = UnityEngine.Random.Range(0, elements.Count);
            return elements[rndIndex];
        }
    }
}