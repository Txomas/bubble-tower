using Game.Core.Bubbles;
using UnityEngine;

namespace Game.Core.Level
{
    public struct BubbleChanged
    {
        public Vector2Int Index { get; }
        public BubbleColor NewColor { get; }
        
        public BubbleChanged(Vector2Int index, BubbleColor newColor)
        {
            Index = index;
            NewColor = newColor;
        }
    }

    public struct NewLevelGridSet
    {
    }
}