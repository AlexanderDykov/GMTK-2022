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
            var turn = new Turn();
            turn.AssignMoves(playerAssignedMoveCollector.CreateAssignedMove());
            turn.AssignMoves(enemyAssignedMoveCollector.CreateAssignedMove());
            arenaService.ApplyTurn(turn);
            await new ResetTurnCommand().Execute();
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnReadyButtonClick);
        }
    }
}
