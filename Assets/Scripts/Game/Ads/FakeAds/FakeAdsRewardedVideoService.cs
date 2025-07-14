using System;
using Game.Popups;
using Zenject;

namespace Game.Ads.FakeAds
{
	public class FakeAdsRewardedVideoService : IAdVideoService
	{
		[Inject] private IPopupsManager _popupsManager;

		public bool IsVideoReady => true;
		public bool IsVideoRequested => true;
		public bool IsWatching => false;

		public void LoadVideo()
		{
		}

		public void ShowVideo(AdPlacement placement, Action<bool> callback)
		{
			_popupsManager.ShowPopup(PopupType.FakeAd, PopupMode.Additive, callback);
		}
	}
}