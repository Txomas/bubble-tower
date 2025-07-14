using Game.Core.Bubbles;
using UnityEngine;
using Zenject;

namespace Game.Core.Level.Runtime
{
    public class LevelPlayerService
    {
        [Inject] private readonly Camera _camera;
        [Inject] private readonly LevelModel _model;
        [Inject] private readonly IGridService _gridService;
        [Inject] private readonly LevelGridView _gridView;
        [Inject] private readonly ShootingConfig _shootingConfig;
        [Inject] private readonly SignalBus _signalBus;

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
            
            _model.UsePlayerBubble();
            _model.SetState(LevelState.Shooting);
                
            return true;

        }

        public void FinishShooting(BubbleData data, Vector2Int shootIndex)
        {
            _model.AddBubble(shootIndex, data);

            var isSuccess = false;
            
            var cluster = _gridService.GetColorCluster(shootIndex, data.Color);

            if (cluster.Count >= _shootingConfig.MinToPop)
            {
                _model.RemoveBubbles(cluster, false);
                isSuccess = true;
            }

            if (data.Type is BubbleType.Bomb)
            {
                var bombCluster = _gridService.GetNeighborsCluster(shootIndex);
                _model.RemoveBubbles(bombCluster, false);
                isSuccess = true;
            }
            
            var floatingBubbles = _gridService.GetFloatingBubbles();
            _model.RemoveBubbles(floatingBubbles, true);

            if (isSuccess)
            {
                _signalBus.Fire<SuccessfullyShoot>();
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