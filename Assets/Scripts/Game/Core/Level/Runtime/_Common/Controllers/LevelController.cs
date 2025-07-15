using System.Collections;
using System.Collections.Generic;
using Game.Popups;
using Game.Saving;
using Zenject;
using Zenject.Helpers;

namespace Game.Core.Level.Runtime
{
    public class LevelController : BaseController, ILevelManager
    {
        [Inject] private readonly LevelService _service;
        [Inject] private readonly LevelModel _model;
        [Inject] private readonly PlayerStatsModel _playerStatsModel;
        [Inject] private readonly IPopupsManager _popupsManager;
        [Inject] private readonly SavingService _savingService;
        private readonly List<BaseController> _levelControllers = new();

        protected override void OnInitialized()
        {
            CreateController<LevelGridController>();
            
            _levelControllers.Add(CreateController<TowerController>());
            _levelControllers.Add(CreateController<LevelInterfaceController>());
            _levelControllers.Add(CreateController<LevelPlayerController>());
            
            SetLevelControllersActive(false);
            
            ResetData();
            
            Subscribe<LevelStateChanged>(OnLevelStateChanged);
        }

        private void ResetData()
        {
            var levelData = _service.GetCurrentLevelData();
            _model.SetData(levelData);
        }

        public void StartLevel()
        {
            SetLevelControllersActive(true);
            
            _model.SetState(LevelState.Idle);
        }

        public void CompleteLevel()
        {
            _playerStatsModel.NextLevel();
            FinishLevel();
        }

        public void FailLevel()
        {
            _playerStatsModel.FailLevel();
            FinishLevel();
        }
        
        private void FinishLevel()
        {
            SetLevelControllersActive(false);
            _savingService.Save(_playerStatsModel);
            
            FireSignal<LevelFinished>();
            
            ResetData();
            
            // TODO: show interstitial ad here
        }

        public void RestartLevel()
        {
            var levelData = _service.GetCurrentLevelData();
            _model.SetData(levelData);
            StartLevel();
        }
        
        private void SetLevelControllersActive(bool isActive)
        {
            foreach (var controller in _levelControllers)
            {
                controller.IsEnabled = isActive;
            }
        }
        
        private void OnLevelStateChanged(LevelStateChanged stateChanged)
        {
            switch (stateChanged.State)
            {
                case LevelState.Completed:
                    // TODO: spent left bubbles
                    _popupsManager.ShowPopup(PopupType.Win);
                    break;
                case LevelState.Failed:
                    _popupsManager.ShowPopup(PopupType.Fail);
                    break;
            }
        }
    }
}