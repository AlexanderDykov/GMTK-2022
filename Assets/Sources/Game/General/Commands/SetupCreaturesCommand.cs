namespace Game.General.Commands
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Services;
    using Views;
    using static UnityEngine.Object;

    public class SetupCreaturesCommand : ICommand
    {
        private readonly IEnemyProvider enemyProvider;

        public SetupCreaturesCommand(IEnemyProvider enemyProvider)
        {
            this.enemyProvider = enemyProvider;
        }

        public UniTask Execute()
        {
            enemyProvider.Next();
            var enemyView = FindObjectsOfType<CreatureView>().FirstOrDefault(x => x.CompareTag("Enemy"));
            if (enemyView != null)
            {
                enemyView.ApplyConfig(enemyProvider.Current);
            }

            return UniTask.CompletedTask;
        }
    }
}