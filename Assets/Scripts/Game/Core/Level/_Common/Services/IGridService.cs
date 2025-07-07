using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IGridService
    {
        Vector3 GetCellPosition(Vector2Int cellIndexes);
        List<Vector2Int> GetUnconnectedCells();
        (int col, int row)[] GetNeighbors(int col, int row);
    }
}