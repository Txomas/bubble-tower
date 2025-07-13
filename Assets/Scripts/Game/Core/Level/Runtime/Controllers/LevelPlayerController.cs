using DG.Tweening;
using Game.Core.Bubbles;
using Game.Input;
using UnityEngine;
using Zenject;
using Zenject.Helpers;

namespace Game.Core.Level.Runtime
{
    public class LevelPlayerController : BaseController
    {
        [Inject] private readonly LevelPlayerService _service;
        [Inject] private readonly BubblesConfig _bubblesConfig;
        [Inject] private readonly ShootingConfig _shootingConfig;
        [Inject] private readonly LevelGridView _gridView;
        
        private BubbleView _playerBubble;
        private BubbleColor _currentColor;

        protected override void OnInitialized()
        {
            Subscribe<NewLevelGridSet>(CreatePlayerBubble);
            Subscribe<PointerTappedSignal>(OnPointerTap);
        }

        private void CreatePlayerBubble()
        {
            _currentColor = _service.GetNewPlayerBubbleColor();
            _playerBubble = Object.Instantiate(_bubblesConfig.BubblePrefab, _gridView.PlayerBubbleContainer);
            _playerBubble.Color = _bubblesConfig.GetBubbleColor(_currentColor);
        }
        
        private void OnPointerTap(PointerTappedSignal signal)
        {
            if (_service.TryShootBubble(signal.ScreenPosition, out var shootIndex, out var shootTarget))
            {
                _gridView.AddBubble(_playerBubble, shootIndex);
                _playerBubble.transform
                    .DOLocalMove(shootTarget, _shootingConfig.ShootSpeed)
                    .SetSpeedBased(true)
                    .OnComplete(() =>
                    {
                        _service.FinishShooting(_currentColor, shootIndex);
                        CreatePlayerBubble();
                    });
            }
        }
    }
}