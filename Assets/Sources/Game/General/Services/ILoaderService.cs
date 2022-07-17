namespace Game.General.Services
{
    using UnityEngine;

    public interface ILoaderService
    {
        void Show();

        void Hide();
    }

    class LoaderService : ILoaderService
    {
        private GameObject loader;

        public void Show()
        {
            if (loader == null)
            {
                var prefab = Resources.Load<GameObject>("Loader");
                loader = Object.Instantiate(prefab);
            }

            loader.SetActive(true);
        }

        public void Hide()
        {
            if (loader != null)
            {
                loader.SetActive(false);
            }
        }
    }
}