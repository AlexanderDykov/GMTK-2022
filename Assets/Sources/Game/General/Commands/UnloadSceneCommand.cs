namespace Game.General.Commands
{
    using Cysharp.Threading.Tasks;
    using UnityEngine.SceneManagement;

    public class UnloadSceneCommand : ICommand
    {
        private readonly string sceneName;

        public UnloadSceneCommand(string sceneName)
        {
            this.sceneName = sceneName;
        }

        public async UniTask Execute()
        {
            await SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}