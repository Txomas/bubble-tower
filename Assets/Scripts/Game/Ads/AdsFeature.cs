using Game.Ads.FakeAds;
using Game.Popups;
using Zenject.Helpers;

namespace Game.Ads
{
    public class AdsFeature : BaseFeatureInstaller<AdsFeature>
    {
        protected override void OnFeatureEnabled()
        {
            BindChildController<AdsButtonController>();

            FakeAdsFeature.Install(Container);
            
            BindChildControllerWithId<IPopupController, AdLoaderPopupController>(PopupType.AdLoader);
        }
    }
}