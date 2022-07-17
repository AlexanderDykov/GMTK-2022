namespace Game.General.Views
{
    using System;
    using System.Collections.Generic;
    using BodyParts;
    using Effects;
    using Services;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;
    using Random = UnityEngine.Random;

    public class CreatureView : MonoBehaviour
    {
        [Inject]
        private IArenaService arenaService;

        [Inject]
        private IPlayerProvider playerProvider;

        [Inject]
        private IEnemyProvider enemyProvider;

        [SerializeField]
        private Slider healthBar;

        [SerializeField]
        private Transform bodiesParent;

        [SerializeField]
        private DicesPanel _dicesPanel;

        [SerializeField]
        private BodyPartView _bodyPartViewPrefab;

        [SerializeField]
        private bool isPlayer;

        private Creature _creature;

        private string ID => isPlayer ? playerProvider.Id : enemyProvider.Id;

        private List<BodyPartView> bodyParts = new List<BodyPartView>();

        public void ApplyConfig(CreatureConfig creatureConfig)
        {
            _creature = new Creature(creatureConfig, ID);
            _creature.CurrentHealthChanged += OnCurrentHealthChanged;

            arenaService.Add(ID, _creature);
            healthBar.maxValue = creatureConfig.MaxHealth;
            UpdateHealthBar(creatureConfig.MaxHealth);

            foreach (var creatureConfigBodyPart in creatureConfig.BodyParts)
            {
                var bodyPart = Instantiate(_bodyPartViewPrefab, bodiesParent);
                bodyParts.Add(bodyPart);
                bodyPart.Create(creatureConfigBodyPart, ID);
            }

            ApplyDices();
        }

        public void ResetCreature()
        {
            ApplyDices();
            foreach (var bodyPartView in bodyParts)
            {
                bodyPartView.ResetBodyPart();
            }
        }


        private void ApplyDices()
        {
            if (_dicesPanel != null)
            {
                var dices = new List<DiceType>();
                foreach (var creatureConfigDice in _creature.Config.Dices)
                {
                    dices.Add(creatureConfigDice.Random());
                }

                _dicesPanel.Setup(ID, dices);
            }
        }

        private void OnCurrentHealthChanged(int currentHealth)
        {
            UpdateHealthBar(currentHealth);
        }

        public void UpdateHealthBar(int health)
        {
            healthBar.value = health;
        }

        private void OnDestroy()
        {
            if (_creature != null)
                _creature.CurrentHealthChanged -= OnCurrentHealthChanged;
        }
    }
}
