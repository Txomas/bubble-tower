using Game.Core.Bubbles;
using UnityEngine;

namespace Game.Core.Level
{
    public struct BubbleChanged
    {
        public Vector2Int Index { get; }
        public BubbleColor NewColor { get; }
        public bool IsDropped { get; }

        public BubbleChanged(Vector2Int index, BubbleColor newColor, bool isDropped = false)
        {
            Index = index;
            NewColor = newColor;
            IsDropped = isDropped;
        }
    }

    public struct NewLevelGridSet
    {
    }
}