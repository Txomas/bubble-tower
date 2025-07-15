using System;
using System.Collections;
using Game.Popups;
using UnityEngine;

namespace Game.Ads.FakeAds
{
	public class FakeAdPopupController : BasePopupController<FakeAdPopupView, Action<bool>>
	{
		private const float SkipDelay = 5f;
		
		private bool _isAdsRewarded;
		private Action<bool> _callback;

		protected override void SetData(Action<bool> popupData)
		{
			_callback = popupData;
		}

		protected override void OnPopupActivated()
		{
			_isAdsRewarded = false;
			_view.SetSkipButtonEnabled(Application.isEditor);
			StartCoroutine(TimerCoroutine());
		}

		protected override void OnCloseClicked()
		{
			base.OnCloseClicked();
			
			_isAdsRewarded = true;
		}

		protected override void OnPopupHidden()
		{
			base.OnPopupHidden();
			
			_callback?.Invoke(_isAdsRewarded);
		}

		private IEnumerator TimerCoroutine()
		{
			var seconds = 0f;

			while (seconds < SkipDelay)
			{
				seconds += Time.deltaTime;
				_view.SetTimer(Mathf.CeilToInt(SkipDelay - seconds));
				yield return null;
			}
			
			_view.SetSkipButtonEnabled(true);
		}
	}
}