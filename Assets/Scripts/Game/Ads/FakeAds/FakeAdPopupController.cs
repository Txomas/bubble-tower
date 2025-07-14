using System;
using System.Collections;
using Game.Popups;
using UnityEngine;

namespace Game.Ads.FakeAds
{
	public class FakeAdPopupController : BasePopupController<FakeAdPopupView, Action<bool>>
	{
		private const float SkipDelay = 5f;
		
		private Action<bool> _callback;

		protected override void SetData(Action<bool> popupData)
		{
			_callback = popupData;
		}

		protected override void OnPopupActivated()
		{
			_view.SetSkipButtonEnabled(Application.isEditor);
			StartCoroutine(TimerCoroutine());
		}

		protected override void OnCloseClicked()
		{
			base.OnCloseClicked();
			
			_callback?.Invoke(true);
			_callback = null;
		}

		protected override void OnPopupHidden()
		{
			base.OnPopupHidden();
			
			_callback?.Invoke(false);
		}

		private IEnumerator TimerCoroutine()
		{
			var seconds = 0f;

			while (seconds < SkipDelay)
			{
				seconds += Time.unscaledDeltaTime;
				_view.SetTimer(Mathf.CeilToInt(SkipDelay - seconds));
				yield return null;
			}
			
			_view.SetSkipButtonEnabled(true);
		}
	}
}