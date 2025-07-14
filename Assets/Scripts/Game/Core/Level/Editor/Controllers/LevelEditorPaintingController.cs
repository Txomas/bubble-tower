using Game.Core.Bubbles;
using Game.Input;
using Zenject;
using Zenject.Helpers;

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
            Subscribe<PointerMovedSignal>(OnPointerMoved);
            Subscribe<NextClickedSignal>(OnNextClicked);
            Subscribe<PreviousClickedSignal>(OnPreviousClicked);
        }

        protected override void OnDisabled()
        {
            UnsubscribeAll();
        }

        private void OnPointerMoved(PointerMovedSignal signal)
        {
            var world = _cameraView.ScreenPointToWorld(signal.ScreenPosition);
            var index = _gridService.WorldToGridIndex(world);

            if (index.x >= 0 && index.x < _gridConfig.Columns &&
                index.y >= 0 && index.y < _gridConfig.Rows)
            {
                var bubbleData = new BubbleData(BubbleType.Default, (BubbleColor)_view.SelectedColor);
                _model.ChangeBubbleColor(index, bubbleData);
            }
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