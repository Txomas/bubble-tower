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
        
        protected ConcreteIdBinderGeneric<T> Bind<T>()
        {
            return Container.Bind<T>();
        }
        
        protected ConcreteIdArgConditionCopyNonLazyBinder BindFromComponentInHierarchy<T>()
        {
            return Bind<T>().FromComponentInHierarchy().AsSingle();
        }
        
        protected ConcreteIdArgConditionCopyNonLazyBinder BindInterfacesTo<T>()
        {
            return Container.BindInterfacesTo<T>().AsSingle();
        }
        
        protected ConcreteIdArgConditionCopyNonLazyBinder BindSingleton<T>()
        {
            return Bind<T>().AsSingle();
        }
        
        protected ConcreteIdArgConditionCopyNonLazyBinder BindSingletonAndInterfaces<T>()
        {
            return BindInterfacesAndSelfTo<T>().AsSingle();
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
        
        protected ArgConditionCopyNonLazyBinder BindChildControllerWithId<TInterface, TController>(object id) 
            where TController : BaseController, TInterface
        {
            return Bind<TInterface>().WithId(id).To<TController>().AsTransient();
        }
        
        protected DeclareSignalIdRequireHandlerAsyncTickPriorityCopyBinder DeclareSignal<TSignal>()
        {
            return Container.DeclareSignal<TSignal>();
        }
        
        protected DeclareSignalAsyncTickPriorityCopyBinder DeclareOptionalSignal<TSignal>()
        {
            return DeclareSignal<TSignal>().OptionalSubscriber();
        }
    }
}