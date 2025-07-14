using Game.Popups;
using Zenject.Helpers;

namespace Game.Ads.FakeAds
{
	public class FakeAdsFeature : BaseFeatureInstaller<FakeAdsFeature>
	{
		protected override void OnFeatureEnabled()
		{
			BindInterfacesTo<FakeAdsRewardedVideoService>();
			BindChildControllerWithId<IPopupController, FakeAdPopupController>(PopupType.FakeAd);
		}
	}
}