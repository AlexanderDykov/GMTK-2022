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

        [SerializeField]
        private Slider healthBar;

        [SerializeField]
        private Transform bodiesParent;

        [SerializeField]
        private DicesPanel _dicesPanel;

        [SerializeField]
        private BodyPartView _bodyPartViewPrefab;

        private Creature _creature;
        string id = Guid.NewGuid().ToString();
        private CreatureConfig _creatureConfig;

        public void ApplyConfig(CreatureConfig creatureConfig)
        {
            _creature = new Creature(creatureConfig.MaxHealth, id);
            _creature.CurrentHealthChanged += OnCurrentHealthChanged;
            _creatureConfig = creatureConfig;

            arenaService.Add(id, _creature);
            healthBar.maxValue = creatureConfig.MaxHealth;
            UpdateHealthBar(creatureConfig.MaxHealth);

            foreach (var creatureConfigBodyPart in creatureConfig.BodyParts)
            {
                var bodyPart = Instantiate(_bodyPartViewPrefab, bodiesParent);
                bodyPart.Create(creatureConfigBodyPart, id);
            }

            ApplyDices();
        }

        public void ApplyDices()
        {
            if (_dicesPanel != null)
            {
                var dices = new List<DiceType>();
                foreach (var creatureConfigDice in _creatureConfig.Dices)
                {
                    dices.Add(creatureConfigDice.Random());
                }

                _dicesPanel.Setup(id, dices);
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