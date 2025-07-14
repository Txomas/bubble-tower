using Game.Ads;
using Game.Popups;
using Zenject;

namespace Game.Core.MainFlow
{
    public class WinPopupController : BasePopupController<WinPopupView>
    {
        [Inject] private readonly EconomyConfig _economyConfig;
        [Inject] private readonly PlayerStatsModel _playerStatsModel;
        [Inject] private readonly ILevelManager _levelManager;
        private bool _isAdsRewarded;

        protected override void OnViewCreated()
        {
            _view.SetRewards(_economyConfig.CoinsForWin, _economyConfig.CoinsForWin * _economyConfig.AdsWinMultiplier);
            
            CreateController<AdsButtonController>(_view.AdsRewardButton, AdPlacement.Win)
                .Success += OnAdsRewardSuccess;
        }

        protected override void OnPopupActivated()
        {
            _isAdsRewarded = false;
        }

        private void OnAdsRewardSuccess()
        {
            _isAdsRewarded = true;
            HidePopup();
        }

        protected override void OnHidePopup()
        {
            var reward = _economyConfig.CoinsForWin;
            
            if (_isAdsRewarded)
            {
                reward *= _economyConfig.AdsWinMultiplier;
            }
            
            _playerStatsModel.AddCoins(reward);
        }

        protected override void OnPopupHidden()
        {
            _levelManager.CompleteLevel();
        }
    }
}