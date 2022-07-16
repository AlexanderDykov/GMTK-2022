namespace Game.General.Views
{
    using Commands;
    using UnityEngine;

    public class EntryPoint : MonoBehaviour
    {
        private async void Start()
        {
            await new LoadSceneCommand("GameScene").Execute();
            await new SetupCreaturesCommand().Execute();
        }
    }
}