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

        private async void Start()
        {
            await new LoadSceneCommand("GameScene").Execute();
            await new SetupPlayerCommand(playerProvider).Execute();
            await new SetupCreaturesCommand(enemyProvider).Execute();
        }
    }
}