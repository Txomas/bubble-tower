namespace Game.Core.Level.Runtime
{
    public class RuntimeLevelFeature : BaseLevelFeature<RuntimeLevelFeature, LevelModel, GridService, LevelGridController>
    {
        protected override void OnFeatureEnabled()
        {
            BindFromComponentInHierarchy<LevelInterfaceView>();
            
            BindSingleton<LevelService>();
            
            BindRootController<LevelController>();
            BindChildController<LevelInterfaceController>();

            DeclareSignal<PlayerBubblesChanged>();
        }
    }
}