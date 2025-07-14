using Zenject.Helpers;

namespace Game.Core.Level.LevelEditor
{
    public class LevelEditorFeature : BaseLevelFeature<LevelEditorFeature, LevelEditorModel, LevelEditorGridService, LevelEditorGridController>
    {
        protected override void OnFeatureEnabled()
        {
            base.OnFeatureEnabled();

            BindFromComponentInHierarchy<LevelEditorCameraView>();
            BindFromComponentInHierarchy<LevelEditorView>();

            BindSingleton<LevelDataService>();

            BindRootController<LevelEditorCameraController>();
            BindRootController<LevelEditorController>();
            BindChildControllerWithId<IActivatable, LevelEditorPaintingController>(LevelViewMode.Editor);
            BindChildControllerWithId<IActivatable, TowerController>(LevelViewMode.Preview);

            DeclareSignal<LevelViewModeChanged>();
        }
    }
}