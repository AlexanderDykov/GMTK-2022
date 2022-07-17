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

        [SerializeField]
        private Button goToMenu;

        [SerializeField]
        private Button nextCreature;
        
        [Inject]
        private IPlayerAssignedMoveCollector playerAssignedMoveCollector;

        [Inject]
        private IArenaService arenaService;

        [Inject]
        private IPlayerProvider playerProvider;
        
        [Inject]
        private IEnemyProvider enemyProvider;

        [Inject]
        private ILoaderService loaderService;

        private void Start()
        {
            DisableAllButtons();
            button.gameObject.SetActive(true);
            button.onClick.AddListener(OnReadyButtonClick);
            goToMenu.onClick.AddListener(OnGoToMenuClick);
            nextCreature.onClick.AddListener(OnNextCreatureClick);
        }

        private async void OnNextCreatureClick()
        {
            loaderService.Show();
            await new UnloadSceneCommand("GameScene").Execute();
            await new LoadSceneCommand("GameScene").Execute();
            await new SetupPlayerCommand(playerProvider, arenaService).Execute();
            await new SetupCreaturesCommand(enemyProvider).Execute();
            loaderService.Hide();
        }

        private async void OnGoToMenuClick()
        {
            enemyProvider.Clear();
            await new UnloadSceneCommand("GameScene").Execute();
        }

        private void DisableAllButtons()
        {
            button.gameObject.SetActive(false);
            goToMenu.gameObject.SetActive(false);
            nextCreature.gameObject.SetActive(false);
        }

        private async void OnReadyButtonClick()
        {
            var arena = arenaService.Get();
            DisableAllButtons();
            var turn = new Turn();
            turn.AssignMoves(playerAssignedMoveCollector.CreateAssignedMove());
            foreach (var creature in arena.Creatures.Values)
            {
                var strategy = creature.Config.ChooseMovesStrategy;
                if (strategy != null)
                {
                    turn.AssignMoves(strategy.ChooseMoves(creature, arena, turn));
                }
            }
            arenaService.ApplyTurn(turn);

            if (arenaService.Creatures.Count == 1)
            {
                if (arenaService.Creatures.Last().Key == playerProvider.Id)
                {
                    Debug.LogError("You win");
                    enemyProvider.Next();
                    if (enemyProvider.Current == null)
                    {
                        goToMenu.gameObject.SetActive(true);
                    }
                    else
                    {
                        nextCreature.gameObject.SetActive(true);
                    }
                }
                else
                {
                    arenaService.ClearArena();
                    Debug.LogError("You loose");
                    goToMenu.gameObject.SetActive(true);
                }

                return;
            }

            await new ResetTurnCommand().Execute();
            button.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnReadyButtonClick);
            goToMenu.onClick.RemoveListener(OnGoToMenuClick);
            nextCreature.onClick.RemoveListener(OnNextCreatureClick);
        }
    }
}