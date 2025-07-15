using System.Collections.Generic;
using Game.Core.Bubbles;
using UnityEngine;

namespace Game.Core.Level
{
    public interface IGridService
    {
        Vector3 IndexToLocalPos(Vector2Int cellIndexes);
        Vector2Int WorldToGridIndex(Vector3 worldPos);
        List<Vector2Int> GetFloatingBubbles();
        bool TryGetFurthestFreeCell(Vector3 origin, Vector3 target, out Vector2Int cellIndex);
        HashSet<Vector2Int> GetColorCluster(Vector2Int startIndex, BubbleColor color);
        IEnumerable<Vector2Int> GetNeighborsCluster(Vector2Int index);
        float GetMaxGridHeight();
    }
}