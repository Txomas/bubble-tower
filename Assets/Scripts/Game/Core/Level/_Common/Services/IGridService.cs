using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Level
{
    public interface IGridService
    {
        Vector3 GetCellPosition(Vector2Int cellIndexes);
        List<Vector2Int> GetUnconnectedCells();
        (int col, int row)[] GetNeighbors(int col, int row);
        (int col, int row) FindNearestCell(Vector3 worldPos);
    }
}