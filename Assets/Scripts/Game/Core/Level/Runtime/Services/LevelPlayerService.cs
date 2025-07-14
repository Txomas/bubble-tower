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
        [Inject] private readonly IGridService _gridService;
        [Inject] private readonly LevelGridView _gridView;
        [Inject] private readonly ShootingConfig _shootingConfig;
        private Random _playerBubblesRandom;
        
        public BubbleColor GetNewPlayerBubbleColor()
        {
            _playerBubblesRandom ??= new Random();
            return _model.Bubbles.Values.Distinct().ToList().GetRandom(_playerBubblesRandom);
        }

        public bool TryShootBubble(Vector2 clickPosition, out Vector2Int shootIndex, out Vector3 shootTarget)
        {
            shootTarget = Vector3.zero;
            shootIndex = Vector2Int.zero;
            
            if (_model.State is not LevelState.Idle)
            {
                return false;
            }
            
            var ray = _camera.ScreenPointToRay(clickPosition);
            
            // TODO: not safe to check only for one collider hit
            if (!Physics.Raycast(ray, out var hit))
            {
                return false;
            }

            var tapWorldPos = hit.point;
            var shootOrigin = _gridView.PlayerBubbleContainer.position;
            // var surfaceNormal = (tapWorldPos - towerTransform.position).normalized;
            // var toPoint = shootOrigin - tapWorldPos;
            // var projectedOrigin = shootOrigin - Vector3.Dot(toPoint, surfaceNormal) * surfaceNormal;

            if (!_gridService.TryGetFurthestFreeCell(shootOrigin, tapWorldPos, out shootIndex))
            {
                return false;
            }

            shootTarget = _gridService.IndexToLocalPos(shootIndex);
            
            _model.RemovePlayerBubble();
            _model.SetState(LevelState.Shooting);
                
            return true;

        }

        public void FinishShooting(BubbleColor color, Vector2Int shootIndex)
        {
            _model.ChangeBubbleColor(shootIndex, color);
            
            var cluster = _gridService.GetCluster(shootIndex, color);

            if (cluster.Count >= _shootingConfig.MinToPop)
            {
                foreach (var index in cluster)
                {
                    _model.ChangeBubbleColor(index, BubbleColor.None);
                }
                
                var floatingBubbles = _gridService.GetFloatingBubbles();
            
                foreach (var index in floatingBubbles)
                {
                    _model.ChangeBubbleColor(index, BubbleColor.None);
                }
            }
            
            if (_model.Bubbles.Count == 0)
            {
                _model.SetState(LevelState.Completed);
            }
            else if (_model.PlayersBubblesLeft == 0)
            {
                _model.SetState(LevelState.Failed);
            }
            else
            {
                _model.SetState(LevelState.Idle);
            }
        }
    }
}