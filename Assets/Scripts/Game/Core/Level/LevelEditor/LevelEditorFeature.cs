using Zenject.Helpers;

namespace Game.Core.Level.LevelEditor
{
    public class LevelEditorFeature : BaseFeatureInstaller<LevelEditorFeature>
    {
        protected override void OnFeatureEnabled()
        {
            BindFromComponentInHierarchy<LevelEditorCameraView>();
            BindFromComponentInHierarchy<LevelEditorView>();
            BindFromComponentInHierarchy<LevelGridView>();

            BindSingleton<LevelModel>();
            BindSingleton<LevelEditorModel>();
            BindSingleton<LevelDataService>();
            BindInterfacesAndSelfTo<LevelEditorGridService>().AsSingle();

            BindRootController<LevelEditorCameraController>();
            BindRootController<LevelEditorController>();
            BindChildController<LevelEditorGridController>();
            BindChildController<TowerController>();
            BindChildController<LevelEditorPaintingController>();

            DeclareSignal<BubbleChanged>();
            DeclareSignal<LevelViewModeChanged>();
            DeclareSignal<NewLevelGridSet>();
        }
    }
}