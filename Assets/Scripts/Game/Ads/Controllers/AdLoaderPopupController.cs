using System.Collections;
using Game.Popups;
using UnityEngine;
using Zenject;

namespace Game.Ads
{
	public class AdLoaderPopupController : BasePopupController<AdLoaderPopupView>
	{
		[Inject] private readonly IAdVideoService _adVideoService;
		
		protected override void OnPopupActivated()
		{
			StartCoroutine(WaitForLoadCoroutine());
		}

		private IEnumerator WaitForLoadCoroutine()
		{
			yield return new WaitUntil(() => _adVideoService.IsVideoReady);
			HidePopup();
		}
	}
}