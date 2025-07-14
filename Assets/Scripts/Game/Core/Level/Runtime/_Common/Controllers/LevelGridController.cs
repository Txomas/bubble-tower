using DG.Tweening;
using Game.Core.Bubbles;
using Zenject;

namespace Game.Core.Level.Runtime
{
    public class LevelGridController : BaseLevelGridController
    {
        [Inject] private readonly LevelGridView _gridView;
        [Inject] private readonly IGridService _service;
        [Inject] private readonly ShootingConfig _shootingConfig;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            Subscribe<BubbleRemoved>(OnBubbleRemoved);
        }

        private void OnBubbleRemoved(BubbleRemoved removed)
        {
            var index = removed.Index;
            
            if (removed.IsDropped)
            {
                var bubble = GetBubbleTransform(index);
                bubble
                    .DOMoveY(_gridView.PlayerBubbleContainer.position.y, _shootingConfig.ShootSpeed)
                    .SetSpeedBased(true)
                    .OnComplete(() => RemoveBubble(index));
            }
            else
            {
                RemoveBubble(index);
            }
        }
    }
}