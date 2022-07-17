namespace Game.General.Views
{
    using Commands;
    using Services;
    using UnityEngine;
    using Zenject;

    public class EntryPoint : MonoBehaviour
    {
        [Inject]
        private IPlayerProvider playerProvider;

        [Inject]
        private IEnemyProvider enemyProvider;

        [Inject]
        private ILoaderService loaderService;

        [Inject]
        private IArenaService arenaService;

        private async void Start()
        {
            loaderService.Show();
            await new LoadSceneCommand("GameScene").Execute();
            await new SetupPlayerCommand(playerProvider, arenaService).Execute();
            await new SetupCreaturesCommand(enemyProvider).Execute();

            loaderService.Hide();
        }
    }
}