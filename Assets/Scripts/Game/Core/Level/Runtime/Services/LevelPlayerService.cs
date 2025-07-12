using System.Linq;
using Common.Extensions;
using Game.Core.Bubbles;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Game.Core.Level.Runtime
{
    public class LevelPlayerService
    {
        [Inject] private readonly Camera _camera;
        [Inject] private readonly LevelModel _model;
        private Random _playerBubblesRandom;
        
        public BubbleColor GetNewPlayerBubbleColor()
        {
            _playerBubblesRandom ??= new Random();
            return _model.Bubbles.Values.Distinct().ToList().GetRandom(_playerBubblesRandom);
        }

        public bool TryShootBubble(Vector2 clickPosition, out Vector3 shootTarget)
        {
            var ray = _camera.ScreenPointToRay(clickPosition);
            
            // TODO: not safe to check only for one collider hit
            if (!Physics.Raycast(ray, out var hit))
            {
                shootTarget = Vector3.zero;
                return false;
            }

            var hitPoint = hit.point;
            
            var (col, row) = FindNearestFreeCell(hitPoint);

            _model.RemovePlayerBubble();

            // 6. Assign to grid
            BubbleColor color = _levelService.GetNewPlayerBubbleColor();
            _gridService.Set(col, row, color);
            _signals.Fire(new BubbleChanged { Position = new Vector2Int(col, row), NewColor = color });
        }
        
        private (int col, int row) FindNearestFreeCell(Vector3 worldPos)
        {
            float radius = _gridConfig.Radius;
            float cellSize = _gridConfig.CellSize;
            var verticalStep = cellSize * 0.75f;

            // 1. Calculate angle around the tower
            var angle = Mathf.Atan2(worldPos.z, worldPos.x);
            if (angle < 0f) angle += Mathf.PI * 2f;

            int colCount = _view.Columns;
            var anglePerCol = 2f * Mathf.PI / colCount;

            // 2. Get approximate column
            var col = Mathf.RoundToInt(angle / anglePerCol) % colCount;

            // 3. Calculate row from height
            var y = -worldPos.y; // Because bubbles grow downward in world space
            var row = Mathf.RoundToInt(y / verticalStep);

            // Clamp inside grid bounds
            row = Mathf.Clamp(row, 0, _view.Rows - 1);
            col = (col + colCount) % colCount;

            return (col, row);
        }
    }
}