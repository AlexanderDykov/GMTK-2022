namespace Game.General.Services
{
    using System.Collections.Generic;
    using Effects;
    using UnityEngine;
    using Zenject;

    public interface IEnemyAssignedMoveCollector
    {
        Dictionary<Target, List<Move>> CreateAssignedMove();
    }

    public class EnemyAssignedMoveCollector : IEnemyAssignedMoveCollector
    {
        [Inject]
        private IEnemyProvider enemyProvider;

        [Inject]
        private IPlayerProvider playerProvider;


        public Dictionary<Target, List<Move>> CreateAssignedMove()
        {
            var result = new Dictionary<Target, List<Move>>();

            foreach (var currentDice in enemyProvider.Current.Dices)
            {
                var randomDice = currentDice.Random();
                var isTargetEnemy = Random.Range(0f, 1f) >= 0.5f;
                var targetId = isTargetEnemy ? enemyProvider.Id : playerProvider.Id;
                var targetParts =
                    isTargetEnemy ? enemyProvider.Current.BodyParts : playerProvider.PlayerConfig.BodyParts;

                if (isTargetEnemy)
                {
                    Debug.LogError("Enemy defends his " + bodyPart + " with value = " + randomDice);
                }
                else
                {
                    Debug.LogError("Enemy attack players " + bodyPart + " with value = " + randomDice);
                }

                result.Add(new Target()
                {
                    Id = targetId,
                    BodyPart = targetParts.RandomElement()
                }, new List<Move>()
                {
                    new()
                    {
                        SourceId = enemyProvider.Id,
                        DiceTypes = new List<DiceType>
                        {
                            randomDice
                        }
                    }
                });
            }

            return result;
        }
    }
}