using Game.Popups;
using Zenject.Helpers;

namespace Game.Core.MainFlow
{
    public class MainFlowFeature : BaseFeatureInstaller<MainFlowFeature>
    {
        protected override void OnFeatureEnabled()
        {
            BindFromComponentInHierarchy<MainMenuView>();
            BindRootController<MainMenuController>();
            
            BindChildControllerWithId<IPopupController, FailPopupController>(PopupType.Fail);
            BindChildControllerWithId<IPopupController, WinPopupController>(PopupType.Win);
        }
    }
}