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
            if(SceneManager.sceneCount > 1)
            {
                await SceneManager.UnloadSceneAsync(sceneName);
            }
        }
    }
}