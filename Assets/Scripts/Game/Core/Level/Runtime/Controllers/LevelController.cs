using Zenject;
using Zenject.Helpers;

namespace Game.Core.Level.Runtime
{
    public class LevelController : BaseController
    {
        [Inject] private readonly LevelService _service;
        [Inject] private readonly LevelModel _model;

        protected override void OnInitialized()
        {
            CreateController<LevelGridController>();
            CreateController<TowerController>();
            CreateController<LevelInterfaceController>();
            CreateController<LevelPlayerController>();
            
            RestartLevel();
        }

        private void RestartLevel()
        {
            var levelData = _service.GetCurrentLevelData();
            _model.SetData(levelData);
        }
    }
}