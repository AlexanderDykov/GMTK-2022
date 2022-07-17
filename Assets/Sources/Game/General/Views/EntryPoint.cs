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

        [Inject]
        private IStartGameService startGameService;

        private async void Start()
        {
            startGameService.Start();
        }
    }
}