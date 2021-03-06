namespace Game.General.Views
{
    using System;
    using System.Collections.Generic;
    using BodyParts;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using Effects;
    using Services;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;
    using Random = UnityEngine.Random;

    public class CreatureView : MonoBehaviour
    {
        [Inject]
        private IArenaService arenaService;

        [Inject]
        private ISoundService soundService;

        [SerializeField]
        private Image icon;

        [SerializeField]
        private Slider healthBar;

        [SerializeField]
        private Transform bodiesParent;

        [SerializeField]
        private DicesPanel _dicesPanel;

        [SerializeField]
        private BodyPartView _bodyPartViewPrefab;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private TMP_Text damageLable;

        [SerializeField]
        private TMP_Text hpLabel;

        private Creature _creature;

        private List<BodyPartView> bodyParts = new List<BodyPartView>();

        private void Awake()
        {
            animator.keepAnimatorControllerStateOnDisable = true;
        }

        public void ApplyConfig(Creature creature)
        {
            _creature = creature;
            icon.sprite = Resources.Load<Sprite>(creature.Config.SpriteName);
            _creature.CurrentHealthChanged += OnCurrentHealthChanged;

            arenaService.Add(_creature.Id, _creature);
            healthBar.maxValue = _creature.Config.MaxHealth;
            UpdateHealthBar(_creature.CurrentHealth);

            foreach (var creatureConfigBodyPart in _creature.Config.BodyParts)
            {
                var bodyPart = Instantiate(_bodyPartViewPrefab, bodiesParent);
                bodyParts.Add(bodyPart);
                bodyPart.Create(creatureConfigBodyPart, _creature);
            }

            ApplyDices();
        }

        public async void ResetCreature()
        {
            ApplyDices();
            foreach (var bodyPartView in bodyParts)
            {
                bodyPartView.ResetBodyPart();
            }

            await UniTask.WaitForEndOfFrame();
            bodiesParent.gameObject.SetActive(false);
            bodiesParent.gameObject.SetActive(true);
            // var cache = bodiesParent.transform.position;
            // bodiesParent.DOMove(cache, 0f);
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
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
                soundService.Play(SFX.DiceRoll);
                _dicesPanel.Setup(_creature.Id, dices);
            }
        }

        private void OnCurrentHealthChanged(int currentHealth, int damage)
        {
            damageLable.SetText("-" + damage);
            animator.SetTrigger("DamageReceive");
            UpdateHealthBar(currentHealth);
        }

        public void UpdateHealthBar(int health)
        {
            if (hpLabel != null)
            {
                hpLabel.text = health.ToString();
            }

            healthBar.value = health;
        }

        private void OnDestroy()
        {
            if (_creature != null)
                _creature.CurrentHealthChanged -= OnCurrentHealthChanged;
        }
    }
}