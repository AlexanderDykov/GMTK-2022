namespace Game.General.Views
{
    using UnityEngine;
    using UnityEngine.UI;

    public class SpellBookButton : MonoBehaviour
    {
        [SerializeField]
        private Sprite openBook;

        [SerializeField]
        private Sprite closedBook;

        [SerializeField]
        private Button button;

        [SerializeField]
        private Button bg;

        [SerializeField]
        private GameObject book;

        private void Start()
        {
            if (PlayerPrefs.GetInt("FirstShow", 0) == 0)
            {
                PlayerPrefs.SetInt("FirstShow", 1);
                button.image.sprite = openBook;
                book.gameObject.SetActive(true);
            }
            else
            {
                button.image.sprite = closedBook;
                book.gameObject.SetActive(false);
            }

            button.onClick.AddListener(OnClick);
            bg.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            button.image.sprite = book.activeSelf ? closedBook : openBook;
            book.SetActive(!book.activeSelf);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
            bg.onClick.RemoveListener(OnClick);
        }
    }
}