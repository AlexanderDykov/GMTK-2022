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

        private IArenaService arenaService;

        public SetupPlayerCommand(IPlayerProvider playerProvider, IArenaService arenaService)
        {
            this.playerProvider = playerProvider;
            this.arenaService = arenaService;
        }

        public UniTask Execute()
        {
            var playerConfig = playerProvider.PlayerConfig;

            var playerView = Object.FindObjectsOfType<CreatureView>().FirstOrDefault(x => x.CompareTag("Player"));
            if (playerView != null)
            {
                var player = new Creature(playerConfig, playerProvider.Id);
                if (arenaService.Creatures.ContainsKey(player.Id))
                {
                    player = arenaService.Creatures[player.Id];
                }

                playerView.ApplyConfig(player);
            }

            return UniTask.CompletedTask;
        }
    }
}