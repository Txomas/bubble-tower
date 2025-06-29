namespace Zenject.Helpers
{
    public abstract class BaseFeatureInstaller<TInstaller> : Installer<TInstaller> where TInstaller : Installer<TInstaller>
    {
        public override void InstallBindings()
        {
            if (IsEnabled)
            {
                OnFeatureEnabled();
            }
            else
            {
                OnFeatureDisabled();
            }
        }

        protected abstract void OnFeatureEnabled();

        protected virtual void OnFeatureDisabled()
        {
        }
        
        protected ScopeConcreteIdArgConditionCopyNonLazyBinder BindFromComponentInChildren<T>()
        {
            return Container.Bind<T>().FromComponentInChildren();
        }
        
        protected ConcreteIdBinderGeneric<T> Bind<T>()
        {
            return Container.Bind<T>();
        }
        
        protected FromBinderNonGeneric BindInterfacesAndSelfTo<T>()
        {
            return Container.BindInterfacesAndSelfTo<T>();
        }
        
        protected ConcreteIdArgConditionCopyNonLazyBinder BindRootController<TController>() 
            where TController : BaseController
        {
            return BindInterfacesAndSelfTo<TController>().AsSingle();
        }
        
        protected ConcreteIdArgConditionCopyNonLazyBinder BindChildController<TController>() 
            where TController : BaseController
        {
            return Bind<TController>().AsTransient();
        }
    }
}