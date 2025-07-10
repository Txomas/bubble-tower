using Game.Core.Bubbles;
using UnityEngine;

namespace Game.Core.Level
{
    public struct BubbleChanged
    {
        public Vector2Int Position { get; }
        public BubbleColor NewColor { get; }
        
        public BubbleChanged(Vector2Int position, BubbleColor newColor)
        {
            Position = position;
            NewColor = newColor;
        }
    }

    public struct LevelViewModeChanged
    {
    }

    public struct NewLevelGridSet
    {
    }
}