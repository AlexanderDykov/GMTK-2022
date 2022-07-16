namespace Game.General.Effects
{

    public enum DiceType
    {
        One = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
    }

    public static class DiceTypeHelper
    {
        public static int GetValue(this DiceType diceType)
        {
            switch (diceType)
            {
                case DiceType.One:
                case DiceType.Two:
                case DiceType.Three:
                case DiceType.Four:
                case DiceType.Five:
                case DiceType.Six:
                    return (int) diceType;
                default:
                    return 0;
            }
        }
    }
}
