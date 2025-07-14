using Game.Core.Bubbles;
using UnityEngine;

namespace Game.Core.Level
{
    public struct BubbleChanged
    {
        public Vector2Int Index { get; }
        public BubbleData Data { get; }

        public BubbleChanged(Vector2Int index, BubbleData data)
        {
            Index = index;
            Data = data;
        }
    }
    
    public struct BubbleRemoved
    {
        public Vector2Int Index { get; }
        public bool IsDropped { get; }

        public BubbleRemoved(Vector2Int index, bool isDropped = false)
        {
            Index = index;
            IsDropped = isDropped;
        }
    }

    public struct NewLevelGridSet
    {
    }
}