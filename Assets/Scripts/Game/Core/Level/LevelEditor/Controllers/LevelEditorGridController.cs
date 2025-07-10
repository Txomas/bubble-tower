using Game.Core.Bubbles;
using UnityEngine;
using Zenject;

namespace Game.Core.Level.LevelEditor
{
    public class LevelEditorGridController : LevelGridController
    {
        [Inject] private readonly LevelEditorModel _model;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            Subscribe<LevelViewModeChanged>(Rebuild);
        }

        protected override bool ShouldCreateCell(Vector2Int indexes, out BubbleColor color)
        {
            return base.ShouldCreateCell(indexes, out color) || _model.ViewMode is LevelViewMode.Editor;
        }
    }
}