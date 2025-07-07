using UnityEngine;
using Zenject.Helpers;

namespace Game
{
    public class LevelEditorFeature : BaseFeatureInstaller<LevelEditorFeature>
    {
        protected override void OnFeatureEnabled()
        {
            BindFromComponentInHierarchy<Camera>();
            BindFromComponentInHierarchy<LevelEditorView>();
            BindFromComponentInHierarchy<LevelGridView>();

            BindSingleton<LevelModel>();
            BindSingleton<LevelEditorModel>();
            BindSingleton<LevelDataService>();
            BindInterfacesAndSelfTo<LevelEditorGridService>().AsSingle();

            BindRootController<LevelEditorGridController>();
            BindRootController<LevelEditorController>();

            DeclareSignal<BubbleChanged>();
            DeclareSignal<LevelViewModeChanged>();
            DeclareSignal<NewLevelGridSet>();
        }
    }
}