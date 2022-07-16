namespace Game.General.Commands
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using Views;

    public class ResetTurnCommand : ICommand
    {
        public UniTask Execute()
        {
            var creatures = Object.FindObjectsOfType<CreatureView>();
            foreach (var creatureView in creatures)
            {
                creatureView.ApplyDices();
            }

            return UniTask.CompletedTask;
        }
    }
}