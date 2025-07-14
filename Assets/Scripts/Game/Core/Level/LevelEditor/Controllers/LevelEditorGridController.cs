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
            Subscribe<BubbleRemoved>(OnBubbleRemoved);
        }

        protected override bool ShouldCreateBubble(Vector2Int indexes, out BubbleData data)
        {
            if (base.ShouldCreateBubble(indexes, out data))
            {
                return true;
            }

            if (_model.ViewMode is LevelViewMode.Editor)
            {
                data = new BubbleData(BubbleType.Default, BubbleColor.Any);
                return true;
            }

            return false;
        }
        
        private void OnBubbleRemoved(BubbleRemoved removed)
        {
            UpdateBubbleColor(removed.Index, new BubbleData(BubbleType.Default, BubbleColor.Any));
        }
    }
}