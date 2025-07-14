namespace Zenject.Helpers
{
    public abstract class BaseMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.BindInterfacesAndSelfTo<AsyncProcessor>().AsSingle();
        }
    }
}