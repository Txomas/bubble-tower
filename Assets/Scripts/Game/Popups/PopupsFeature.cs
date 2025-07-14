using Zenject.Helpers;

namespace Game.Popups
{
    public class PopupsFeature : BaseFeatureInstaller<PopupsFeature>
    {
        protected override void OnFeatureEnabled()
        {
            BindFromComponentInHierarchy<PopupsRootView>();
            BindInterfacesTo<PopupsManager>();
        }
    }
}