using Common.Extensions;
using Game.Core.Bubbles;
using Game.Input;
using UnityEngine;
using Zenject;
using Zenject.Helpers;
using Vector2 = UnityEngine.Vector2;

namespace Game.Core.Level.LevelEditor
{
    public class LevelEditorPaintingController : BaseController
    {
        [Inject] private readonly LevelEditorView _view;
        [Inject] private readonly LevelEditorCameraView _cameraView;
        [Inject] private readonly LevelEditorModel _model;
        [Inject] private readonly IGridService _gridService;
        [Inject] private readonly LevelGridConfig _gridConfig;
        
        protected override void OnEnabled()
        {
            Subscribe<SecondaryClickedSignal>(OnSecondaryClicked);
            Subscribe<PointerMovedSignal>(OnPointerMoved);
            Subscribe<NextClickedSignal>(OnNextClicked);
            Subscribe<PreviousClickedSignal>(OnPreviousClicked);
        }

        protected override void OnDisabled()
        {
            UnsubscribeAll();
        }
        
        private void OnSecondaryClicked(SecondaryClickedSignal signal)
        {
            if (TryGetBubbleIndex(signal.ScreenPosition, out var index))
            {
                _model.RemoveBubble(index, false);
            }
        }

        private void OnPointerMoved(PointerMovedSignal signal)
        {
            if (TryGetBubbleIndex(signal.ScreenPosition, out var index))
            {
                var selectedColorOffset = EnumExtensions.GetEnumList<BubbleColor>().Count - _view.ColorsOptionsCount;
                var color = (BubbleColor)(_view.SelectedColor + selectedColorOffset);
                var bubbleData = new BubbleData(BubbleType.Default, color);
                _model.ChangeBubbleColor(index, bubbleData);
            }
        }

        private bool TryGetBubbleIndex(Vector2 screenPosition, out Vector2Int index)
        {
            var world = _cameraView.ScreenPointToWorld(screenPosition);
            index = _gridService.WorldToGridIndex(world);

            return index.x >= 0 && index.x < _gridConfig.Columns &&
                   index.y >= 0 && index.y < _gridConfig.Rows;
        }
        
        private void OnNextClicked(NextClickedSignal signal)
        {
            _view.SetSelectedColor(_view.SelectedColor + 1);
        }

        private void OnPreviousClicked(PreviousClickedSignal signal)
        {
            _view.SetSelectedColor(_view.SelectedColor - 1);
        }
    }
}