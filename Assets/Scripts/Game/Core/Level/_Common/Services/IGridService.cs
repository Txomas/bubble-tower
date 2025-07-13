using System.Collections.Generic;
using Game.Core.Bubbles;
using UnityEngine;

namespace Game.Core.Level
{
    public interface IGridService
    {
        Vector3 IndexToLocalPos(Vector2Int cellIndexes);
        List<Vector2Int> GetFloatingBubbles();
        bool TryGetFurthestFreeCell(Vector3 origin, Vector3 target, out Vector2Int cellIndex);
        HashSet<Vector2Int> GetCluster(Vector2Int startIndex, BubbleColor color);
    }
}