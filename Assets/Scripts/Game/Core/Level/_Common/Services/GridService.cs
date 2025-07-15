using System.Collections.Generic;
using System.Linq;
using Game.Core.Bubbles;
using UnityEngine;
using Zenject;

namespace Game.Core.Level
{
    public class GridService : IGridService
    {
        [Inject] protected readonly LevelGridConfig _gridConfig;
        [Inject] private readonly IGridModel _levelModel;
        [Inject] private readonly LevelGridView _gridView;
        
        // TODO: cache?
        protected float CellSize => _gridConfig.CellSize;
        protected int Columns => _gridConfig.Columns;
        protected int Rows => _gridConfig.Rows;
        protected float HeightStep => _gridConfig.HeightStep;
        
        public virtual Vector3 IndexToLocalPos(Vector2Int cellIndexes)
        {
            var col = cellIndexes.x;
            var row = cellIndexes.y;

            var radius = _gridConfig.Radius;
            
            var angle = col / (float)Columns * Mathf.PI * 2f;

            if (row % 2 == 1)
            {
                angle += Mathf.PI * 2f / (2f * Columns);
            }
            
            var x = Mathf.Cos(angle) * radius;
            var z = Mathf.Sin(angle) * radius;
            var y = row * HeightStep;

            return new Vector3(x, -y, z);
        }
        
        public virtual Vector2Int WorldToGridIndex(Vector3 worldPos)
        {
            var localPos = GetLocalPoint(worldPos);
            
            var row = Mathf.RoundToInt(-localPos.y / HeightStep);
            var angle = Mathf.Atan2(localPos.z, localPos.x);
            
            if (row % 2 == 1)
            {
                angle -= Mathf.PI * 2f / (2f * Columns);
            }
            
            if (angle < 0f)
            {
                angle += Mathf.PI * 2f;
            }

            var col = Mathf.RoundToInt(angle / (Mathf.PI * 2f) * Columns);
            
            return new Vector2Int(col, row);
        }

        protected Vector3 GetLocalPoint(Vector3 worldPoint)
        {
            return _gridView.CellsContainer.InverseTransformPoint(worldPoint);
        }
        
        public List<Vector2Int> GetFloatingBubbles()
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
                
                foreach (var np in GetNeighbors(pos.x, pos.y))
                {
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
        
        public List<Vector2Int> GetNeighbors(int col, int row)
        {
            var offsets = row % 2 == 0
                ? new[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }, { -1, -1 }, { -1, 1 } }
                : new[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 1 } };
            var neighbors = new List<Vector2Int>();
            
            for (var i = 0; i < 6; i++)
            {
                var nc = WrapCol(col + offsets[i, 0]);
                var nr = row + offsets[i, 1];
                
                if (nr >= 0 && nr < Rows)
                {
                    neighbors.Add(new Vector2Int(nc, nr));
                }
            }
            
            return neighbors;
        }
        
        private int WrapCol(int col)
        {
            return (col + Columns) % Columns;
        }

        public bool TryGetFurthestFreeCell(Vector3 origin, Vector3 target, out Vector2Int cellIndex)
        {
            var direction = (target - origin).normalized;

            var positionToCheck = origin;
            Vector2Int? lastEmpty = null;
            var lastIndex = Vector2Int.zero;

            while (lastIndex is { x: >= 0, y: >= 0 })
            {
                lastIndex = WorldToGridIndex(positionToCheck);
                
                if (_levelModel.HasBubble(lastIndex))
                {
                    break;
                }
                
                lastEmpty = lastIndex;
                positionToCheck += direction * _gridConfig.HeightStepFactor;
            }
            
            if (lastEmpty.HasValue)
            {
                cellIndex = lastEmpty.Value;
                return true;
            }
            else
            {
                cellIndex = Vector2Int.zero;
                return false;
            }
        }
        
        public HashSet<Vector2Int> GetColorCluster(Vector2Int startIndex, BubbleColor color)
        {
            var result = new HashSet<Vector2Int>();
            var queue = new Queue<Vector2Int>();

            queue.Enqueue(startIndex);
            result.Add(startIndex);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var neighbors = GetNeighbors(current.x, current.y);
                
                foreach (var neighbor in neighbors)
                {
                    if (!result.Contains(neighbor) && 
                        _levelModel.Bubbles.TryGetValue(neighbor, out var bubbleData) && 
                        bubbleData.Color == color)
                    {
                        result.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return result;
        }

        public IEnumerable<Vector2Int> GetNeighborsCluster(Vector2Int index)
        {
            var neighbors = GetNeighbors(index.x, index.y);
            return neighbors.Where(neighbor => _levelModel.HasBubble(neighbor));
        }
        
        public float GetMaxGridHeight()
        {
            return _levelModel.Bubbles.Select(bubble => bubble.Key.y).Prepend(0).Max() +
                   _gridConfig.MaxAdditionalRows * HeightStep;
        }
    }
}