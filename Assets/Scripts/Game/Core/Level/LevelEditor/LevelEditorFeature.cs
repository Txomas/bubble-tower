using UnityEngine;

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
            BindChildController<LevelEditorPaintingController>();

            DeclareSignal<LevelViewModeChanged>();
        }
    }
}