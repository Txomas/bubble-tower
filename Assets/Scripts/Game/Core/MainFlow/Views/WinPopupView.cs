using Common.Components;
using Game.Popups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.MainFlow
{
    public class WinPopupView : BasePopupView
    {
        [SerializeField] private TMP_Text _bonusRewardText;
        [SerializeField] private Button _adsRewardButton;
        [SerializeField] private TextButton _continueButton;
        
        public Button AdsRewardButton => _adsRewardButton;
        
        public void SetRewards(int defaultReward, int adsReward)
        {
            _bonusRewardText.text = adsReward.ToString();
            _continueButton.SetText(defaultReward.ToString());
        }
    }
}