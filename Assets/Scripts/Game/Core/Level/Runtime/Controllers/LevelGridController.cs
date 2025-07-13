using Game.Core.Bubbles;
using Zenject;

namespace Game.Core.Level.Runtime
{
    public class LevelGridController : BaseLevelGridController
    {
        [Inject] private readonly IGridService _service;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            Subscribe<ShootingFinished>(OnShootingFinished);
        }

        protected override void OnBubbleChanged(BubbleChanged changedData)
        {
            var color = changedData.NewColor;
            var index = changedData.Index;
            
            if (color is BubbleColor.None)
            {
                RemoveBubble(index);
            }
            else
            {
                SetBubbleColor(index, color);
            }
        }
        
        private void OnShootingFinished(ShootingFinished _)
        {
            var floatingBubbles = _service.GetFloatingBubbles();
            
            foreach (var index in floatingBubbles)
            {
                RemoveBubble(index);
            }
        }
    }
}