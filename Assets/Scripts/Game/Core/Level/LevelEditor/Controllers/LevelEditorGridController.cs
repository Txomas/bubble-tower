using Game.Core.Bubbles;
using UnityEngine;
using Zenject;

namespace Game.Core.Level.LevelEditor
{
    public class LevelEditorGridController : BaseLevelGridController
    {
        [Inject] private readonly LevelEditorModel _model;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            Subscribe<LevelViewModeChanged>(Rebuild);
        }

        protected override void OnBubbleChanged(BubbleChanged changedData)
        {
            SetBubbleColor(changedData.Index, changedData.NewColor);
        }

        protected override bool ShouldCreateBubble(Vector2Int indexes, out BubbleColor color)
        {
            return base.ShouldCreateBubble(indexes, out color) || _model.ViewMode is LevelViewMode.Editor;
        }
    }
}