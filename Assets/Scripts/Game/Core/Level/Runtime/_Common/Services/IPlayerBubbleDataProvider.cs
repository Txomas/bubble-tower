using Game.Core.Bubbles;

namespace Game.Core.Level.Runtime
{
    public interface IPlayerBubbleDataProvider
    {
        BubbleData GetNewBubbleData();
    }
}