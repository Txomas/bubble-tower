using Game.Input;
using Zenject.Helpers;

namespace Game.Core.Level.LevelEditor
{
    public class LevelEditorInstaller : BaseMonoInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            InputFeature.Install(Container);

#if UNITY_EDITOR
            LevelEditorFeature.Install(Container);
#endif
        }
    }
}