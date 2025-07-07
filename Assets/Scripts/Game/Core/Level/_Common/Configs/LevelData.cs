using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "LevelData", menuName = ConfigsPaths.Configs + "LevelData")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<Vector2Int, BubbleColor> _coloredCells = new();

        public IReadOnlyDictionary<Vector2Int, BubbleColor> ColoredCells => _coloredCells;

        public void SetColoredCells(IReadOnlyDictionary<Vector2Int, BubbleColor> coloredCells)
        {
            _coloredCells = new SerializedDictionary<Vector2Int, BubbleColor>(coloredCells);
        }
    }
}