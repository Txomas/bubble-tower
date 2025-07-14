using Zenject;
using Zenject.Helpers;

namespace Game.Core.MainFlow
{
    public class MainMenuController : BaseController
    {
        [Inject] private readonly MainMenuView _view;
        [Inject] private readonly PlayerStatsModel _playerStatsModel;
        [Inject] private readonly ILevelManager _levelManager;

        protected override void OnInitialized()
        {
            _view.StartButtonClicked.AddListener(OnStartButtonClicked);
            
            UpdateCurrentLevel();
            Subscribe<LevelChanged>(UpdateCurrentLevel);
        }
        
        private void OnStartButtonClicked()
        {
            _levelManager.StartLevel();
            _view.gameObject.SetActive(false);
        }

        private void UpdateCurrentLevel()
        {
            _view.SetCurrentLevel(_playerStatsModel.Level);
        }
    }
}