namespace Game.General.Views
{
    using System;
    using System.Collections.Generic;
    using BodyParts;
    using Effects;
    using UnityEngine;
    using UnityEngine.UI;
    using Random = UnityEngine.Random;

    public class CreatureView : MonoBehaviour
    {
        [SerializeField]
        private Slider healthBar;

        [SerializeField]
        private Transform bodiesParent;

        [SerializeField]
        private DicesPanel _dicesPanel;

        [SerializeField]
        private BodyPartView _bodyPartViewPrefab;

        private Arena _arena = new Arena();

        private Creature _creature;

        public void ApplyConfig(CreatureConfig creatureConfig)
        {
            var id = Guid.NewGuid().ToString();
            _creature = new Creature(creatureConfig.MaxHealth, id);
            _creature.CurrentHealthChanged += OnCurrentHealthChanged;

            _arena.Creatures.Add(id, _creature);
            UpdateHealthBar(creatureConfig.MaxHealth);
            var dices = new List<DiceType>();
            foreach (var creatureConfigDice in creatureConfig.Dices)
            {
                dices.Add(creatureConfigDice.Random());
            }
            foreach (var creatureConfigBodyPart in creatureConfig.BodyParts)
            {
                var bodyPart = Instantiate(_bodyPartViewPrefab, bodiesParent);
                bodyPart.Create(creatureConfigBodyPart, id);
            }

            _dicesPanel.Setup(id, dices);
        }

        private void OnCurrentHealthChanged(int currentHealth)
        {
            UpdateHealthBar(currentHealth);
        }

        public void UpdateHealthBar(int health)
        {
        }

        private void OnDestroy()
        {
            _creature.CurrentHealthChanged -= OnCurrentHealthChanged;
        }
    }
}