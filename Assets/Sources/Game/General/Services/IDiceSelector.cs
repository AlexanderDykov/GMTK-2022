namespace Game.General.Services
{
    using System;
    using Views;

    public interface IDiceSelector
    {
        DiceElement Selected { get; }

        event Action<DiceElement> ElementSelected;
        event Action ElementDeselected;

        void Select(DiceElement element);

        void Deselect();
    }

    public class DiceSelector : IDiceSelector
    {
        public DiceElement Selected { get; private set; }

        public event Action<DiceElement> ElementSelected;
        public event Action ElementDeselected;

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
    }
}