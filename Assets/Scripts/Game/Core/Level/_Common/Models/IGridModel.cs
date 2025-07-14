using System.Collections.Generic;
using Game.Core.Bubbles;
using UnityEngine;

namespace Game.Core.Level
{
    public interface IGridModel
    {
        IReadOnlyDictionary<Vector2Int, BubbleData> Bubbles { get; }
        
        bool HasBubble(Vector2Int index);
    }
}