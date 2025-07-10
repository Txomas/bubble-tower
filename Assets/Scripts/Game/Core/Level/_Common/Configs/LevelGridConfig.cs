using Common.Attributes;
using Game.Core.Bubbles;
using UnityEditor;
using UnityEngine;

namespace Game.Core.Level
{
    [CreateAssetMenu(fileName = nameof(LevelGridConfig), menuName = ConfigsPaths.Game + nameof(LevelGridConfig))]
    public class LevelGridConfig : ScriptableObject
    {
        public BubbleView CellPrefab;
        [CallOnChange(nameof(RecalculateRadius))] public float CellSize = 1f;       // width/height of one hex cell
        [ReadOnly] public float Radius = 5f;         // distance from the tower’s center to the cells
        [CallOnChange(nameof(RecalculateRadius))] public int Columns;
        public int Rows = 10;

        private void RecalculateRadius()
        {
#if UNITY_EDITOR
            Radius = Mathf.Max(0.1f, Columns * CellSize / (2f * Mathf.PI));
            EditorUtility.SetDirty(this);
#endif
        }
    }
}