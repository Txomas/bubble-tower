using System.Collections.Generic;
using System.Linq;
using Game.Core.Bubbles;
using UnityEngine;
using Zenject;

namespace Game.Core.Level
{
    public abstract class BaseLevelModel : IGridModel
    {
        [Inject] protected readonly SignalBus _signalBus;
        private Dictionary<Vector2Int, BubbleData> _bubbles = new();
        
        public IReadOnlyDictionary<Vector2Int, BubbleData> Bubbles => _bubbles;

        public virtual void SetData(LevelData data)
        {
            SetData(data.Bubbles);
        }
        
        public void SetData(IReadOnlyDictionary<Vector2Int, BubbleData> bubbles)
        {
            _bubbles = new Dictionary<Vector2Int, BubbleData>(bubbles);
            _signalBus.Fire<NewLevelGridSet>();
        }
        
        public bool HasBubble(Vector2Int index)
        {
            return _bubbles.ContainsKey(index);
        }
        
        public void AddBubble(Vector2Int index, BubbleData bubbleData)
        {
            if (!_bubbles.TryAdd(index, bubbleData))
            {
                _bubbles[index] = bubbleData;
            }
        }
        
        public void ChangeBubbleColor(Vector2Int index, BubbleData data)
        {
            AddBubble(index, data);
            _signalBus.Fire(new BubbleChanged(index, data));
        }
        
        public void RemoveBubbles(IEnumerable<Vector2Int> indexes, bool isDropped)
        {
            foreach (var index in indexes)
            {
                RemoveBubble(index, isDropped);
            }
        }
        
        public void RemoveBubble(Vector2Int index, bool isDropped)
        {
            if (_bubbles.Remove(index))
            {
                _signalBus.Fire(new BubbleRemoved(index, isDropped));
            }
        }
    }
}