namespace Game.General.Views
{
    using System;
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
        private IArenaService arenaService;

        private void Start()
        {
            button.onClick.AddListener(OnReadyButtonClick);
        }

        private async void OnReadyButtonClick()
        {
            arenaService.ApplyTurn(new Turn
            {
                AssignedMoves = playerAssignedMoveCollector.CreateAssignedMove()
            });
            await new ResetTurnCommand().Execute();
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnReadyButtonClick);
        }
    }
}