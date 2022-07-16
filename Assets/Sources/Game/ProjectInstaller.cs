namespace Game
{
    using General.Services;
    using Zenject;

    public class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IDiceSelector>().To<DiceSelector>().AsSingle();
            Container.Bind<IPlayerAssignedMoveCollector>().To<PlayerAssignedMoveCollector>().AsSingle();
            Container.Bind<IArenaService>().To<ArenaService>().AsSingle();
        }
    }
}