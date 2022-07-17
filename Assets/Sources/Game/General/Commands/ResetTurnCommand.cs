namespace Game.General.Commands
{
    using Cysharp.Threading.Tasks;
    using Services;
    using UnityEngine;
    using Views;

    public class ResetTurnCommand : ICommand
    {
        public async UniTask Execute()
        {
            var creatures = Object.FindObjectsOfType<CreatureView>();
            foreach (var creatureView in creatures)
            {
                creatureView.ResetCreature();
            }

            await UniTask.Delay(2000);
        }
    }
}