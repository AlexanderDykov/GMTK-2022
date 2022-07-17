namespace Game.General.Services
{
    using System;

    public interface IStartTurnService
    {
        event Action TurnStarted;

        void StartTurn();
    }

    class  StartTurnService : IStartTurnService
    {
        public event Action TurnStarted;
        public void StartTurn()
        {
            TurnStarted?.Invoke();
        }
    }
}