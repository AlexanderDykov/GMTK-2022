namespace Game.General.Services
{
    public interface IUIBlocker
    {
        public bool Block { get; set; }
    }

    class UIBlocker : IUIBlocker
    {
        public bool Block { get; set; }
    }
}