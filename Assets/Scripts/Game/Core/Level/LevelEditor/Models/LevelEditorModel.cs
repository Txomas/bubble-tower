using Zenject;

namespace Game.Core.Level.LevelEditor
{
    public class LevelEditorModel
    {
        [Inject] private readonly SignalBus _signalBus;
        
        public LevelViewMode ViewMode { get; private set; }

        public void SetViewMode(LevelViewMode viewMode)
        {
            ViewMode = viewMode;
            _signalBus.Fire<LevelViewModeChanged>();
        }
    }
}