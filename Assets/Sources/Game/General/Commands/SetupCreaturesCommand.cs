namespace Game.General.Commands
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Services;
    using UnityEngine;
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
            var enemyView = FindObjectsOfType<CreatureView>().FirstOrDefault(x => x.CompareTag("Enemy"));
            if (enemyView != null)
            {
                Debug.LogError("You start fighting with " + enemyProvider.Current.SpriteName);
                var enemy = new Creature(enemyProvider.Current, enemyProvider.Id);
                enemyView.ApplyConfig(enemy);
            }

            return UniTask.CompletedTask;
        }
    }
}