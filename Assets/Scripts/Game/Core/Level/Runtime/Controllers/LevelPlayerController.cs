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
        [Inject] private readonly LevelGridView _gridView;
        
        private BubbleView _playerBubble;
        private BubbleColor _currentColor;
        private bool _isShooting;

        protected override void OnInitialized()
        {
            CreatePlayerBubble();
            
            Subscribe<PointerClickedSignal>(OnPointerClick);
        }

        private void CreatePlayerBubble()
        {
            if (_playerBubble != null)
            {
                return;
            }
            
            _currentColor = _service.GetNewPlayerBubbleColor();
            _playerBubble = Object.Instantiate(_bubblesConfig.BubblePrefab, _gridView.PlayerBubbleAnchor);
            _playerBubble.Color = _bubblesConfig.GetBubbleColor(_currentColor);
        }
        
        private void OnPointerClick(PointerClickedSignal signal)
        {
            if (_isShooting)
            {
                return;
            }
            
            if (_service.TryShootBubble(signal.ScreenPosition, out var shootTarget))
            {
                _isShooting = true;
            }

            // 7. Move bubble to cell visually
            MoveBubbleToGrid(_playerBubble, col, row);
            _playerBubble = null;

            // 8. Pop neighbors
            var toPop = _gridService.GetCluster(col, row);
            if (toPop.Count > 1)
            {
                foreach (var pos in toPop)
                    _gridService.Set(pos.x, pos.y, BubbleColor.None);
            }

            // 9. Drop floaters
            var floaters = _gridService.GetUnconnectedCells();
            foreach (var pos in floaters)
                _gridService.Set(pos.x, pos.y, BubbleColor.None);

            // 10. Check game end
            if (_gridService.Current.GetAllFilled().Count() == 0 || _bubblesRemaining == 0)
            {
                Debug.Log("Level complete or failed");
            }

            // Queue next bubble
            CreateNewBubble();
        }

        private void MoveBubbleToGrid(BubbleView bubble, int col, int row)
        {
            Vector3 pos = _view.GridView.CalculatePosition(col, row, _view.SelectedViewMode);
            bubble.transform.position = pos;
        }
    }
}