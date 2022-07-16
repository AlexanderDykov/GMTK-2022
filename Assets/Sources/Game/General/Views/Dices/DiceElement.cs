namespace Game.General.Views
{
    using Cysharp.Threading.Tasks;
    using Effects;
    using Services;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using Zenject;

    public class DiceElement : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private Image border;

        [SerializeField]
        private LayoutElement _layoutElement;

        [Inject]
        private IDiceIconProvider _diceIconProvider;

        [Inject]
        private IDiceSelector _diceSelector;

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
        }

        private void OnDestroy()
        {
            _diceSelector.ElementSelected -= OnElementSelected;
            _diceSelector.ElementDeselected -= OnElementDeselected;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_diceSelector.Selected == this)
            {
                _diceSelector.Deselect();
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _layoutElement.ignoreLayout = true;
                _diceSelector.Select(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                _diceSelector.Deselect();
                _layoutElement.ignoreLayout = false;
                _diceSelector.Remove(this);
            }

        }
    }
}