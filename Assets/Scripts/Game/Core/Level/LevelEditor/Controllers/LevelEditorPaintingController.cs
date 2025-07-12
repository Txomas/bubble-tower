using Game.Core.Bubbles;
using Game.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Zenject.Helpers;

namespace Game.Core.Level.LevelEditor
{
    public class LevelEditorPaintingController : BaseController
    {
        [Inject] private readonly LevelEditorView _view;
        [Inject] private readonly LevelEditorCameraView _cameraView;
        [Inject] private readonly LevelEditorModel _model;
        
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
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            
            var ray = _cameraView.ScreenPointToRay(signal.ScreenPosition);
            
            if (Physics.Raycast(ray, out var hit) && 
                hit.collider.TryGetComponent(out BubbleView view))
            {
                var index = view.GridIndex;
                _model.ChangeBubbleColor(index, (BubbleColor)_view.SelectedColor);
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