using System.Collections.Generic;
using Game.Core.Bubbles;
using UnityEngine;

namespace Game.Core.Level
{
    public interface IGridModel
    {
        IReadOnlyDictionary<Vector2Int, BubbleColor> Bubbles { get; }
        
        bool HasBubble(Vector2Int indexes);
    }
}