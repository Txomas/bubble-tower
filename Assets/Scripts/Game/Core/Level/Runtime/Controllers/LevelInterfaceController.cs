using Zenject;
using Zenject.Helpers;

namespace Game.Core.Level.Runtime
{
    public class LevelInterfaceController : BaseController
    {
        [Inject] private readonly LevelInterfaceView _view;
        [Inject] private readonly LevelModel _model;

        protected override void OnInitialized()
        {
            Subscribe<NewLevelGridSet>(UpdateBubblesCount);
            Subscribe<PlayerBubblesChanged>(UpdateBubblesCount);
        }

        private void UpdateBubblesCount()
        {
            _view.SetBubblesCount(_model.PlayersBubblesLeft);
        }
    }
}