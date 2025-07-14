using System.Collections.Generic;
using Game.Core.Bubbles;
using UnityEngine;
using Zenject;

namespace Game.Core.Level
{
    public abstract class BaseLevelModel : IGridModel
    {
        [Inject] protected readonly SignalBus _signalBus;
        private Dictionary<Vector2Int, BubbleColor> _bubbles = new();
        
        public IReadOnlyDictionary<Vector2Int, BubbleColor> Bubbles => _bubbles;

        public virtual void SetData(LevelData data)
        {
            SetData(data.ColoredCells);
        }
        
        public void SetData(IReadOnlyDictionary<Vector2Int, BubbleColor> coloredCells)
        {
            _bubbles = new Dictionary<Vector2Int, BubbleColor>(coloredCells);
            _signalBus.Fire<NewLevelGridSet>();
        }
        
        public bool HasBubble(Vector2Int indexes)
        {
            return _bubbles.ContainsKey(indexes);
        }
        
        public void ChangeBubbleColor(Vector2Int index, BubbleColor newColor, bool isDropped = false)
        {
            if (_bubbles.TryGetValue(index, out var currentColor) && currentColor == newColor)
            {
                return;
            }

            if (newColor is BubbleColor.None)
            {
                _bubbles.Remove(index);
            }
            else if (!_bubbles.TryAdd(index, newColor))
            {
                _bubbles[index] = newColor;
            }

            _signalBus.Fire(new BubbleChanged(index, newColor, isDropped));
        }
    }
}