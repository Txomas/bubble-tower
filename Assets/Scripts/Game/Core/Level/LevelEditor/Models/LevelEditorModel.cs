using Game.Core.Bubbles;
using UnityEngine;

namespace Game.Core.Level.LevelEditor
{
    public class LevelEditorModel : BaseLevelModel
    {
        public LevelViewMode ViewMode { get; private set; }

        public void SetViewMode(LevelViewMode viewMode)
        {
            ViewMode = viewMode;
            _signalBus.Fire<LevelViewModeChanged>();
        }
    }
}