using Game.Core.Level.Runtime.Combo;
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
            BindInterfacesTo<DefaultPlayerBubbleDataProvider>();
            
            BindRootController<LevelController>();
            BindChildController<LevelInterfaceController>();
            BindChildController<LevelPlayerController>();
            BindChildController<TowerController>();

            DeclareSignal<PlayerBubblesChanged>();
            DeclareSignal<LevelStateChanged>();
            DeclareSignal<SuccessfullyShoot>();
            
            ComboFeature.Install(Container);
        }
    }
}