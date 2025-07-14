using Common.Attributes;
using UnityEditor;
using UnityEngine;

namespace Game.Core.Level
{
    [CreateAssetMenu(fileName = nameof(LevelGridConfig), menuName = ConfigsPaths.Game + nameof(LevelGridConfig))]
    public class LevelGridConfig : ScriptableObject
    {
        [Tooltip("Height between hex cells: CellSize * HeightStepFactor.")]
        public float HeightStepFactor = 0.75f;
        [Tooltip("Width/height of one hex cell in world units. The radius is calculated based on this value.")]
        [CallOnChange(nameof(RecalculateRadius))] public float CellSize = 1f;
        [Tooltip("The distance from the tower’s center to the cells. It is calculated based on the number of columns and cell size.")]
        [ReadOnly] public float Radius = 5f;
        [CallOnChange(nameof(RecalculateRadius))] public int Columns;
        public int Rows = 10;
        
        public float HeightStep => CellSize * HeightStepFactor;

        private void RecalculateRadius()
        {
#if UNITY_EDITOR
            Radius = Mathf.Max(0.1f, Columns * CellSize / (2f * Mathf.PI));
            EditorUtility.SetDirty(this);
#endif
        }
    }
}