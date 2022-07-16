namespace Game.General.Services
{
    using Effects;
    using UnityEngine;

    public interface IDiceIconProvider
    {
        public Sprite GetIcon(DiceType diceType);
    }
}