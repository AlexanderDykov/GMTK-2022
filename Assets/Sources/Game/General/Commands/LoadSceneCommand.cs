namespace Game.General.Commands
{
    using Cysharp.Threading.Tasks;
    using UnityEngine.SceneManagement;

    public class LoadSceneCommand : ICommand
    {
        private readonly string sceneName;

        public LoadSceneCommand(string sceneName)
        {
            this.sceneName = sceneName;
        }

        public async UniTask Execute()
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}