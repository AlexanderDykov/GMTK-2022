namespace Game.General.Commands
{
    using Cysharp.Threading.Tasks;

    public interface ICommand
    {
        UniTask Execute();
    }
}