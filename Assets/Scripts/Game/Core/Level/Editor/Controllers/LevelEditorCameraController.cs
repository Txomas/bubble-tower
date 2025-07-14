using Game.Input;
using Zenject;
using Zenject.Helpers;

namespace Game.Core.Level.LevelEditor
{
    public class LevelEditorCameraController : BaseController
    {
        [Inject] private readonly LevelEditorCameraView _cameraView;
        [Inject] private readonly LevelEditorModel _editorModel;
        [Inject] private readonly LevelEditorInputConfig _inputConfig;

        protected override void OnInitialized()
        {
            UpdateViewMode();
            
            Subscribe<LevelViewModeChanged>(UpdateViewMode);
            Subscribe<MoveSignal>(OnMove);
            Subscribe<ScrollSignal>(OnScroll);
        }

        private void UpdateViewMode()
        {
            _cameraView.SetViewMode(_editorModel.ViewMode);
        }
        
        private void OnMove(MoveSignal signal)
        {
            _cameraView.MoveCamera(signal.Direction * _inputConfig.CameraMoveSensitivity);
        }
        
        private void OnScroll(ScrollSignal signal)
        {
            _cameraView.ZoomCamera(signal.Delta * _inputConfig.CameraZoomSensitivity);
        }
    }
}