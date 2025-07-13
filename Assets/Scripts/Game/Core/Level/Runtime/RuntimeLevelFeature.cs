using UnityEngine;

namespace Game.Core.Level.Runtime
{
    public class RuntimeLevelFeature : BaseLevelFeature<RuntimeLevelFeature, LevelModel, GridService, LevelGridController>
    {
        protected override void OnFeatureEnabled()
        {
            base.OnFeatureEnabled();
            
            BindFromComponentInHierarchy<Camera>();
            BindFromComponentInHierarchy<LevelInterfaceView>();
            
            BindSingleton<LevelService>();
            BindSingleton<LevelPlayerService>();
            
            BindRootController<LevelController>();
            BindChildController<LevelInterfaceController>();
            BindChildController<LevelPlayerController>();

            DeclareSignal<PlayerBubblesChanged>();
        }
    }
}