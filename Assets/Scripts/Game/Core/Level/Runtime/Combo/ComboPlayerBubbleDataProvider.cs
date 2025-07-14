using Game.Core.Bubbles;
using Game.Core.Level.Combo;
using Zenject;

namespace Game.Core.Level.Runtime.Combo
{
    public class ComboPlayerBubbleDataProvider : IPlayerBubbleDataProvider
    {
        [Inject] private readonly ComboModel _model;
        [Inject] private readonly ComboConfig _config;
        [Inject] private readonly IPlayerBubbleDataProvider _defaultProvider;
        
        public BubbleData GetNewBubbleData()
        {
            var type = _config.GetBubbleType(_model.Streak);
            var color = _defaultProvider.GetNewBubbleData().Color;
            return new BubbleData(type, color);
        }
    }
}