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

        protected override void OnBubbleChanged(BubbleChanged changedData)
        {
            var color = changedData.NewColor;
            var index = changedData.Index;
            
            if (color is BubbleColor.None)
            {
                if (changedData.IsDropped)
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
            else
            {
                SetBubbleColor(index, color);
            }
        }
    }
}