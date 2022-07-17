namespace Game.General.Services
{
    using Commands;
    using Cysharp.Threading.Tasks;
    using Zenject;

    public interface IStartGameService
    {
        UniTask Start();
    }

    class StartGameService : IStartGameService
    {
        [Inject]
        private IPlayerProvider playerProvider;

        [Inject]
        private IEnemyProvider enemyProvider;

        [Inject]
        private ILoaderService loaderService;

        [Inject]
        private IArenaService arenaService;

        [Inject]
        private IStartTurnService startTurnService;

        [Inject]
        private IUIBlocker uiBlocker;

        public async UniTask Start()
        {
            uiBlocker.Block = true;
            loaderService.Show();
            await new UnloadSceneCommand("GameScene").Execute();
            await new LoadSceneCommand("GameScene").Execute();
            await new SetupPlayerCommand(playerProvider, arenaService).Execute();
            await new SetupCreaturesCommand(enemyProvider).Execute();
            startTurnService.StartTurn();
            loaderService.Hide();
            await UniTask.Delay(2000);
            uiBlocker.Block = false;
        }
    }
}