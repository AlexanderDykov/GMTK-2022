namespace Game.General.Views
{
    using System;
    using System.Linq;
    using Commands;
    using Services;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class ReadyState : MonoBehaviour
    {
        [SerializeField]
        private Button button;


        [Inject]
        private IPlayerAssignedMoveCollector playerAssignedMoveCollector;

        [Inject]
        private IEnemyAssignedMoveCollector enemyAssignedMoveCollector;


        [Inject]
        private IArenaService arenaService;

        private void Start()
        {
            button.onClick.AddListener(OnReadyButtonClick);
        }

        private async void OnReadyButtonClick()
        {
            var assignedMoves = playerAssignedMoveCollector.CreateAssignedMove();
            var enemyAssignedMoves = enemyAssignedMoveCollector.CreateAssignedMove();

            foreach (var enemyAssignedMove in enemyAssignedMoves)
            {
                var duplicate = assignedMoves.Keys.FirstOrDefault(x =>
                    x.Id == enemyAssignedMove.Key.Id && x.BodyPart == enemyAssignedMove.Key.BodyPart);

                if (duplicate != null)
                {
                    var duplicateWithValue = assignedMoves.FirstOrDefault(x =>
                        x.Key.Id == enemyAssignedMove.Key.Id && x.Key.BodyPart == enemyAssignedMove.Key.BodyPart);
                    duplicateWithValue.Value.AddRange(enemyAssignedMove.Value);
                }
                else
                {
                    assignedMoves.Add(enemyAssignedMove.Key, enemyAssignedMove.Value);
                }
            }

            var turn = new Turn
            {
                AssignedMoves = assignedMoves
            };
            arenaService.ApplyTurn(turn);
            await new ResetTurnCommand().Execute();
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnReadyButtonClick);
        }
    }
}