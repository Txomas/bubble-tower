using Common.Components;
using Game.Popups;
using TMPro;
using UnityEngine;

namespace Game.Ads.FakeAds
{
	public class FakeAdPopupView : BasePopupView
	{
		private const string TimerFormat = "YOU CAN SKIP AD AFTER {0} SEC";
		
		[SerializeField] private TMP_Text _timerText;
		[SerializeField] private TextButton _skipButton;
		
		public void SetTimer(int seconds)
		{
			_timerText.text = string.Format(TimerFormat, seconds);
		}
		
		public void SetSkipButtonEnabled(bool value)
		{
			_skipButton.Interactable = value;
		}
	}
}