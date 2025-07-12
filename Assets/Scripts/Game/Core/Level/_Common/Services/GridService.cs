using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Game.Core.Level
{
    public class GridService : IGridService
    {
        [Inject] protected readonly LevelGridConfig _gridConfig;
        [Inject] private readonly IGridModel _levelModel;
        private float _rotationOffset;
        
        // TODO: cache?
        private int Columns => _gridConfig.Columns;
        private int Rows => _gridConfig.Rows;
        private float HeightStep => _gridConfig.CellSize * 0.75f;
        
        public void SetRotationOffset(float offset)
        {
            _rotationOffset = offset;
        }
        
        public virtual Vector3 GetCellPosition(Vector2Int cellIndexes)
        {
            var col = cellIndexes.x;
            var row = cellIndexes.y;

            var radius = _gridConfig.Radius;
            
            var angle = col / (float)Columns * Mathf.PI * 2f;

            if (row % 2 == 1)
            {
                angle += Mathf.PI * 2f / (2f * Columns);
            }
            
            angle += _rotationOffset;

            var x = Mathf.Cos(angle) * radius;
            var z = Mathf.Sin(angle) * radius;
            var y = row * HeightStep;

            return new Vector3(x, -y, z);
        }
        
        public List<Vector2Int> GetUnconnectedCells()
        {
            var connected = new HashSet<Vector2Int>();
            var queue = new Queue<Vector2Int>();
            
            // Enqueue top row bubbles
            for (var c = 0; c < Columns; c++)
            {
                var pos = new Vector2Int(c, 0);
                
                if (_levelModel.HasBubble(pos))
                {
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
                    
                    if (!connected.Contains(np) && _levelModel.HasBubble(np))
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
                
                if (nr >= 0 && nr < Rows)
                {
                    neighbors.Add((nc, nr));
                }
            }
            
            return neighbors.ToArray();
        }
        
        private int WrapCol(int col)
        {
            return (col + Columns) % Columns;
        }
        
        public (int col, int row) FindNearestCell(Vector3 worldPos)
        {
            var angle = Mathf.Atan2(worldPos.z, worldPos.x);
            angle -= _rotationOffset;
            
            if (angle < 0f)
            {
                angle += Mathf.PI * 2f;
            }

            var anglePerCol = 2f * Mathf.PI / Columns;
            var col = Mathf.RoundToInt(angle / anglePerCol) % Columns;

            var y = -worldPos.y;
            var row = Mathf.RoundToInt(y / HeightStep);

            return (col, row);
        }
    }
}