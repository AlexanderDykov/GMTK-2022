namespace Game.General.Views
{
    using System;
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

        private void OnReadyButtonClick()
        {
            var a = playerAssignedMoveCollector.CreateAssignedMove();
            arenaService.ApplyTurn(new Turn()
            {
                AssignedMoves = a
            });
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnReadyButtonClick);
        }
    }
}