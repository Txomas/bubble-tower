using UnityEngine;

namespace Game.Core.Level.LevelEditor
{
    [CreateAssetMenu(fileName = nameof(LevelEditorInputConfig), menuName = ConfigsPaths.LevelEditor + nameof(LevelEditorInputConfig))]
    public class LevelEditorInputConfig : ScriptableObject
    {
        public float CameraMoveSensitivity = 0.1f;
        public float CameraZoomSensitivity = 0.1f;
    }
}