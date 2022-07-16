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

        [Inject]
        private IDiceIconProvider _diceIconProvider;

        [Inject]
        private IDiceSelector _diceSelector;

        private Transform initialParent;

        private void Awake()
        {
            initialParent = transform.parent;
        }

        private void Start()
        {
            _diceSelector.ElementSelected += OnElementSelected;
            _diceSelector.ElementDeselected += OnElementDeselected;
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
        }

        public void ResetParent()
        {
            transform.parent = initialParent;
        }
    }
}