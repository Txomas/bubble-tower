using DG.Tweening;
using Game.Core.Bubbles;
using UnityEngine;
using Zenject;

namespace Game.Core.Level.Runtime
{
    public class LevelGridController : BaseLevelGridController
    {
        [Inject] private readonly LevelGridView _gridView;
        [Inject] private readonly IGridService _service;
        [Inject] private readonly ShootingConfig _shootingConfig;
        [Inject] private readonly LevelGridConfig _config;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            Subscribe<BubbleRemoved>(OnBubbleRemoved);
            Subscribe<LevelStateChanged>(OnLevelStateChanged);
        }

        protected override void Rebuild()
        {
            base.Rebuild();
            
            _gridView.CellsContainer.localPosition = Vector3.zero;
        }

        private void OnBubbleRemoved(BubbleRemoved removed)
        {
            var index = removed.Index;
            
            if (removed.IsDropped)
            {
                var bubble = GetBubbleTransform(index);
                var endPosition = _gridView.PlayerBubbleContainer.position.y - _shootingConfig.BallsDropOffset;
                bubble
                    .DOMoveY(endPosition, _shootingConfig.ShootSpeed)
                    .SetSpeedBased(true)
                    .SetEase(Ease.InCirc)
                    .OnComplete(() => RemoveBubble(index));
            }
            else
            {
                RemoveBubble(index);
            }
        }
        
        private void OnLevelStateChanged(LevelStateChanged stateChanged)
        {
            if (stateChanged.State == LevelState.Idle)
            {
                var height = _service.GetMaxGridHeight();
                var playerBubblePosition = _gridView.PlayerBubbleContainer.position.y;
                var cellsPosition = playerBubblePosition + height;
                
                if (cellsPosition < _gridView.CellsContainer.position.y)
                {
                    _gridView.CellsContainer
                        .DOMoveY(cellsPosition, _config.CellsContainerMoveSpeed)
                        .SetSpeedBased(true);
                }
            }
        }
    }
}