using Zenject.Helpers;

namespace Game.Input
{
    public class InputFeature : BaseFeatureInstaller<InputFeature>
    {
        protected override void OnFeatureEnabled()
        {
            BindSingleton<InputSystemActions>();
            BindRootController<InputController>();

            DeclareOptionalSignal<PointerMovedSignal>();
            DeclareOptionalSignal<ScrollSignal>();
            DeclareOptionalSignal<NextClickedSignal>();
            DeclareOptionalSignal<PreviousClickedSignal>();
            DeclareOptionalSignal<MoveSignal>();
        }
    }
}