using System.Linq;
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
        [Inject] private readonly BubblesConfig _bubblesConfig;

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

            if (!_gridService.TryGetFurthestFreeCell(shootOrigin, tapWorldPos, out shootIndex))
            {
                return false;
            }

            shootTarget = _gridService.IndexToLocalPos(shootIndex);
            
            _model.UsePlayerBubble();
            _model.SetState(LevelState.Shooting);
            
            _gridView.PlayShootEffect();
                
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
                
                var effect = Object.Instantiate(_bubblesConfig.BubbleBoomEffect, _gridView.CellsContainer);
                effect.transform.localPosition = _gridService.IndexToLocalPos(shootIndex);
            }
            
            var floatingBubbles = _gridService.GetFloatingBubbles();
            
            if (_model.HasBubble(shootIndex) && !_gridService.GetNeighborsCluster(shootIndex).Any())
            {
                floatingBubbles.Add(shootIndex);
            }
            
            _model.RemoveBubbles(floatingBubbles, true);
            
            _signalBus.Fire(new ShotFinished(isSuccess));

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