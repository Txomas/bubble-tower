using Game.Core.Bubbles;
using Zenject;

namespace Game.Core.Level.Runtime
{
    public class LevelGridController : BaseLevelGridController
    {
        [Inject] private readonly IGridService _service;

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
    }
}