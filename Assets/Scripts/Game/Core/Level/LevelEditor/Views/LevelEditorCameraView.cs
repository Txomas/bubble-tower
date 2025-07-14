using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Game.Core.Level.LevelEditor
{
    public class LevelEditorCameraView : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<LevelViewMode, Camera> _cameras;
        private Camera _editorCamera;

        private Camera EditorCamera
        {
            get
            {
                if (_editorCamera == null)
                {
                    _editorCamera = _cameras[LevelViewMode.Editor];
                }
                
                return _editorCamera;
            }
        }
        
        public Vector3 ScreenPointToWorld(Vector2 screenPosition)
        {
            var screenPont = new Vector3(screenPosition.x, screenPosition.y, EditorCamera.transform.position.y);
            return EditorCamera.ScreenToWorldPoint(screenPont);
        }
        
        public void SetViewMode(LevelViewMode viewMode)
        {
            foreach (var (key, value) in _cameras)
            {
                value.gameObject.SetActive(key == viewMode);
            }
        }
        
        public void MoveCamera(Vector2 direction)
        {
            EditorCamera.transform.Translate(direction, Space.Self);
        }

        public void ZoomCamera(float zoomDelta)
        {
            EditorCamera.orthographicSize = Mathf.Max(1, EditorCamera.orthographicSize + zoomDelta);
        }
    }
}