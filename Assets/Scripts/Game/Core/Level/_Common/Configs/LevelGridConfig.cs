using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = nameof(LevelGridConfig), menuName = ConfigsPaths.Configs + nameof(LevelGridConfig))]
    public class LevelGridConfig : ScriptableObject
    {
        public BubbleView CellPrefab;
        public float CellSize = 1f;       // width/height of one hex cell
        public float Radius = 5f;         // distance from the tower’s center to the cells
        
        // TODO: cache?
        public int Columns => Mathf.Max(1, Mathf.FloorToInt(2f * Mathf.PI * Radius / CellSize));
        public int Rows = 10;
    }
}