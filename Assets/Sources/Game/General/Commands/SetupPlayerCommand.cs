namespace Game.General.Commands
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Services;
    using UnityEngine;
    using Views;

    public class SetupPlayerCommand : ICommand
    {
        private IPlayerProvider playerProvider;

        public SetupPlayerCommand(IPlayerProvider playerProvider)
        {
            this.playerProvider = playerProvider;
        }

        public UniTask Execute()
        {
            var player = playerProvider.PlayerConfig;

            var playerView = Object.FindObjectsOfType<CreatureView>().FirstOrDefault(x => x.CompareTag("Player"));
            if (playerView != null)
            {
                playerView.ApplyConfig(player);
            }

            return UniTask.CompletedTask;
        }
    }
}