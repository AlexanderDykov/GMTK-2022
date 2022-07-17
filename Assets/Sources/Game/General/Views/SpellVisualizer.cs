namespace Game.General.Views
{
    using UnityEngine;

    public class SpellVisualizer : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _icon;

        public void Setup(Sprite icon)
        {
            _icon.sprite = icon;
            Destroy(gameObject, 1f);
        }
    }
}