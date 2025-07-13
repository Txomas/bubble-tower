using Zenject.Helpers;

namespace Game.Core.Level
{
    public abstract class BaseLevelFeature<T, TModel, TService, TController> : BaseFeatureInstaller<T> 
        where T : BaseLevelFeature<T, TModel, TService, TController>
        where TModel : IGridModel
        where TService : IGridService
        where TController : BaseLevelGridController
    {
        protected override void OnFeatureEnabled()
        {
            BindFromComponentInHierarchy<LevelGridView>();

            BindSingletonAndInterfaces<TModel>();
            BindSingletonAndInterfaces<TService>();
            
            BindChildController<TController>();
            BindChildController<TowerController>();

            DeclareSignal<BubbleChanged>();
            DeclareSignal<NewLevelGridSet>();
        }
    }
}