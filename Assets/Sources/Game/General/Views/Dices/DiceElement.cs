namespace Game.General.Views
{
    using System;
    using Cysharp.Threading.Tasks;
    using Effects;
    using Services;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using Zenject;

    public class DiceElement : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private Image border;

        [SerializeField]
        private Animator animator;

        [Inject]
        private IDiceIconProvider _diceIconProvider;

        [Inject]
        private IDiceSelector _diceSelector;

        [Inject]
        private IStartTurnService startTurnService;

        private Transform initialParent;

        private void Awake()
        {
            animator.keepAnimatorControllerStateOnDisable = true;
            initialParent = transform.parent;
        }

        private void Start()
        {
            _diceSelector.ElementSelected += OnElementSelected;
            _diceSelector.ElementDeselected += OnElementDeselected;
            startTurnService.TurnStarted += OnTurnStarted;
        }

        private void OnTurnStarted()
        {
        }

        private void OnElementDeselected()
        {
            border.gameObject.SetActive(false);
        }

        private void OnElementSelected(DiceElement selected)
        {
            border.gameObject.SetActive(selected == this);
        }

        [HideInInspector]
        public DiceType diceType;

        [HideInInspector]
        public string sourceId;

        public UniTask SpeenAnimation()
        {
            if (animator != null)
            {
                animator.SetTrigger("DiceRoll");
            }

            return UniTask.CompletedTask;
        }

        public void Apply(DiceType diceType, string sourceId)
        {
            this.diceType = diceType;
            this.sourceId = sourceId;
            image.sprite = _diceIconProvider.GetIcon(diceType);
            ResetParent();
        }

        private void OnDestroy()
        {
            _diceSelector.ElementSelected -= OnElementSelected;
            _diceSelector.ElementDeselected -= OnElementDeselected;
            startTurnService.TurnStarted -= OnTurnStarted;
        }

        public void ResetParent()
        {
            transform.parent = initialParent;
        }
    }
}