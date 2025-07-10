using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Game.Core.Level
{
    public class GridService : IGridService
    {
        [Inject] protected readonly LevelGridConfig _gridConfig;
        [Inject] private readonly LevelModel _levelModel;
        
        public virtual Vector3 GetCellPosition(Vector2Int cellIndexes)
        {
            var col = cellIndexes.x;
            var row = cellIndexes.y;
            var cellSize = _gridConfig.CellSize;

            var radius = _gridConfig.Radius;
            var heightStep = cellSize * 0.75f;

            var colCount = _gridConfig.Columns;
            var baseAngle = col / (float)colCount * Mathf.PI * 2f;

            if (row % 2 == 1)
            {
                baseAngle += Mathf.PI * 2f / (2f * colCount);
            }

            var x = Mathf.Cos(baseAngle) * radius;
            var z = Mathf.Sin(baseAngle) * radius;
            var y = row * heightStep;

            return new Vector3(x, -y, z);

        }
        
        public List<Vector2Int> GetUnconnectedCells()
        {
            var connected = new HashSet<Vector2Int>();
            var queue = new Queue<Vector2Int>();
            
            // Enqueue top row bubbles
            for (var c = 0; c < _gridConfig.Columns; c++)
            {
                if (_levelModel.HasBubble(c, 0))
                {
                    var pos = new Vector2Int(c, 0);
                    queue.Enqueue(pos);
                    connected.Add(pos);
                }
            }
            
            // BFS
            while (queue.Count > 0)
            {
                var pos = queue.Dequeue();
                
                foreach (var (nc, nr) in GetNeighbors(pos.x, pos.y))
                {
                    var np = new Vector2Int(nc, nr);
                    
                    if (!connected.Contains(np) && _levelModel.HasBubble(nc, nr))
                    {
                        connected.Add(np);
                        queue.Enqueue(np);
                    }
                }
            }
            
            // All non-connected
            return _levelModel.Bubbles
                .Select(kv => kv.Key)
                .Where(p => !connected.Contains(p))
                .ToList();
        }
        
        public (int col, int row)[] GetNeighbors(int col, int row)
        {
            var offsets = row % 2 == 0
                ? new[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }, { -1, -1 }, { -1, 1 } }
                : new[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 1 } };
            var neighbors = new List<(int, int)>();
            
            for (var i = 0; i < 6; i++)
            {
                var nc = WrapCol(col + offsets[i, 0]);
                var nr = row + offsets[i, 1];
                
                if (nr >= 0 && nr < _gridConfig.Rows)
                {
                    neighbors.Add((nc, nr));
                }
            }
            
            return neighbors.ToArray();
        }
        
        private int WrapCol(int col)
        {
            return (col + _gridConfig.Columns) % _gridConfig.Columns;
        }
    }
}