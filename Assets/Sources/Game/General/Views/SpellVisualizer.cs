namespace Game.General.Views
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

    public class SpellVisualizer : MonoBehaviour
    {
        [SerializeField]
        private Image[] _icons;

        public void Setup(Sprite icon)
        {
            gameObject.SetActive(true);
            for (var i = 0; i < _icons.Length; i++)
            {
                _icons[i].sprite = icon;
            }

            DOTween.Sequence().SetDelay(0.6f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}