namespace Game.General.Services
{
    using System;
    using Views;

    public interface IDiceSelector
    {
        DiceElement Selected { get; }

        event Action<DiceElement> ElementSelected;
        event Action<DiceElement> ElementRemovedToHand;
        event Action ElementDeselected;

        void Select(DiceElement element);

        void Deselect();
        void Remove(DiceElement diceElement);
    }

    public class DiceSelector : IDiceSelector
    {
        public DiceElement Selected { get; private set; }

        public event Action<DiceElement> ElementSelected;
        public event Action ElementDeselected;
        public event Action<DiceElement> ElementRemovedToHand;

        public void Select(DiceElement element)
        {
            Selected = element;
            ElementSelected?.Invoke(element);
        }

        public void Deselect()
        {
            Selected = null;
            ElementDeselected?.Invoke();
        }

        public void Remove(DiceElement diceElement)
        {
            ElementRemovedToHand?.Invoke(diceElement);
        }
    }
}