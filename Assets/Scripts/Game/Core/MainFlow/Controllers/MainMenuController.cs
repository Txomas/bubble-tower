using Common.Extensions;
using DG.Tweening;
using Game.Core.Level.Runtime;
using UnityEngine;
using Zenject;
using Zenject.Helpers;

namespace Game.Core.MainFlow
{
    public class MainMenuController : BaseController
    {
        [Inject] private readonly MainMenuView _view;
        [Inject] private readonly PlayerStatsModel _playerStatsModel;
        [Inject] private readonly ILevelManager _levelManager;
        [Inject] private readonly UiValuesConfig _uiValuesConfig;

        protected override void OnInitialized()
        {
            _view.StartButtonClicked.AddListener(OnStartButtonClicked);
            
            UpdateCurrentLevel();
            UpdateCoinsCounter();
            UpdateHeartsCounter();
            
            Subscribe<LevelFinished>(_view.Show);
            Subscribe<LevelChanged>(UpdateCurrentLevel);
            Subscribe<CoinsChanged>(AnimatedUpdateCoinsCounter);
            Subscribe<HeartsChanged>(UpdateHeartsCounter);
        }
        
        private void OnStartButtonClicked()
        {
            _levelManager.StartLevel();
            _view.Hide();
        }

        private void UpdateCurrentLevel()
        {
            _view.SetCurrentLevel(_playerStatsModel.Level);
        }

        private void AnimatedUpdateCoinsCounter()
        {
            DOTween.To(
                () => _view.Coins,
                value => _view.SetCoins(Mathf.RoundToInt(value)),
                _playerStatsModel.Coins,
                _uiValuesConfig.CounterTweenDuration);
        }
        
        private void UpdateCoinsCounter()
        {
            _view.SetCoins(_playerStatsModel.Coins);
        }
        
        private void UpdateHeartsCounter()
        {
            _view.SetHearts(_playerStatsModel.Hearts);
        }
    }
}