namespace Game
{
    using General.Services;
    using Zenject;

    public class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IDiceSelector>().To<DiceSelector>().AsSingle();
            Container.Bind<ISpellBookService>().To<SpellBookService>().AsSingle();
            Container.Bind<IArenaService>().To<ArenaService>().AsSingle();
            Container.Bind<IPlayerProvider>().To<PlayerProvider>().AsSingle();
            Container.Bind<IEnemyProvider>().To<EnemyProvider>().AsSingle();
            Container.Bind<ILoaderService>().To<LoaderService>().AsSingle();
        }
    }
}
