namespace Game.General.Views
{
    using System;
    using System.Linq;
    using BodyParts;
    using Commands;
    using Cysharp.Threading.Tasks;
    using Services;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;
    using static UnityEngine.Object;

    public class ReadyState : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private Button goToMenu;

        [SerializeField]
        private Button nextCreature;

        [SerializeField]
        private GameObject endGamePanel;

        [SerializeField]
        private TMP_Text winLoseLable;

        [Inject]
        private IArenaService arenaService;

        [Inject]
        private IPlayerProvider playerProvider;

        [Inject]
        private IEnemyProvider enemyProvider;

        [Inject]
        private IStartGameService startGameService;

        [Inject]
        private IStartTurnService startTurnService;

        [Inject]
        private IUIBlocker uiBlocker;

        [Inject]
        private IDiceSelector diceSelector;

        [Inject]
        private ISoundService soundService;
        
        private void Start()
        {
            DisableAllButtons();
            button.gameObject.SetActive(true);
            button.onClick.AddListener(OnReadyButtonClick);
            goToMenu.onClick.AddListener(OnGoToMenuClick);
            nextCreature.onClick.AddListener(OnNextCreatureClick);
        }

        private void OnNextCreatureClick()
        {
            soundService.Play(SFX.ButtonClick);
            startGameService.Start();
        }

        private void OnGoToMenuClick()
        {
            soundService.Play(SFX.ButtonClick);
            enemyProvider.Restart();
            startGameService.Start();
        }

        private void DisableAllButtons()
        {
            button.gameObject.SetActive(false);
            goToMenu.gameObject.SetActive(false);
            nextCreature.gameObject.SetActive(false);
        }

        private async void OnReadyButtonClick()
        {
            soundService.Play(SFX.ButtonClick);
            uiBlocker.Block = true;
            diceSelector.Deselect();
            
            var arena = arenaService.Get();
            DisableAllButtons();
            var turn = new Turn();
            foreach (var creature in arena.Creatures.Values)
            {
                var strategy = creature.Config.ChooseMovesStrategy;
                if (strategy != null)
                {
                    var chooseMoves = strategy.ChooseMoves(creature, arena, turn);


                    if (creature.Id != playerProvider.Id)
                    {
                        foreach (var chooseMove in chooseMoves)
                        {
                            var bodyPartView = FindObjectsOfType<BodyPartView>()
                                .First(x => x.Id == chooseMove.Key.Id && x.BodyPart == chooseMove.Key.BodyPart);
                            foreach (var move in chooseMove.Value)
                            {
                                foreach (var moveDiceType in move.DiceTypes)
                                {
                                    bodyPartView.AddEnemyDices(moveDiceType, creature.Id, creature.Config.SpriteName);
                                }
                            }
                        }
                    }

                    turn.AssignMoves(chooseMoves);
                }
            }
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            await arenaService.ApplyTurn(turn);

            if (arenaService.Creatures.Count <= 1)
            {
                if (arenaService.Creatures.Count > 0 && arenaService.Creatures.Last().Key == playerProvider.Id)
                {
                    Debug.LogError("You win");
                    enemyProvider.Next();
                    if (enemyProvider.Current == null)
                    {
                        winLoseLable.text = "You win this game!";
                        goToMenu.gameObject.SetActive(true);
                        endGamePanel.gameObject.SetActive(true);
                    }
                    else
                    {
                        winLoseLable.text = "You win this round!";
                        endGamePanel.gameObject.SetActive(true);
                        nextCreature.gameObject.SetActive(true);
                    }
                }
                else
                {
                    arenaService.ClearArena();
                    winLoseLable.text = "You loose!";
                    Debug.LogError("You loose");
                    goToMenu.gameObject.SetActive(true);
                    endGamePanel.gameObject.SetActive(true);
                }

                return;
            }

            await new ResetTurnCommand().Execute();

            gameObject.SetActive(false);
            gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            startTurnService.StartTurn();
            button.gameObject.SetActive(true);
            uiBlocker.Block = false;
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnReadyButtonClick);
            goToMenu.onClick.RemoveListener(OnGoToMenuClick);
            nextCreature.onClick.RemoveListener(OnNextCreatureClick);
        }
    }
}