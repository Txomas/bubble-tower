using System;

namespace Game.Ads
{
	public interface IAdVideoService
	{
		void LoadVideo();
		void ShowVideo(AdPlacement placement, Action<bool> callback);

		bool IsVideoReady { get; }
		bool IsVideoRequested { get; }
		bool IsWatching { get; }
	}
}