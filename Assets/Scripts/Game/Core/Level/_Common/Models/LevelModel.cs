using System.Collections.Generic;
using Game.Core.Bubbles;
using UnityEngine;
using Zenject;

namespace Game.Core.Level
{
    public class LevelModel
    {
        [Inject] private readonly SignalBus _signalBus;
        private Dictionary<Vector2Int, BubbleColor> _bubbles = new();
        
        public IReadOnlyDictionary<Vector2Int, BubbleColor> Bubbles => _bubbles;

        public void SetData(LevelData data)
        {
            SetData(data.ColoredCells);
        }
        
        public void SetData(IReadOnlyDictionary<Vector2Int, BubbleColor> coloredCells)
        {
            _bubbles = new Dictionary<Vector2Int, BubbleColor>(coloredCells);
            _signalBus.Fire<NewLevelGridSet>();
        }
        
        public bool HasBubble(int column, int row)
        {
            return _bubbles.ContainsKey(new Vector2Int(column, row));
        }
        
        public void ChangeBubbleColor(Vector2Int index, BubbleColor newColor)
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

            _signalBus.Fire(new BubbleChanged(index, newColor));
        }
    }
}