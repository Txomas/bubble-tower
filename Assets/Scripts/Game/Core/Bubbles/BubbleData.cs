using System;
using UnityEngine;

namespace Game.Core.Bubbles
{
    [Serializable]
    public class BubbleData
    {
        [field: SerializeField] public BubbleType Type { get; private set; }
        [field: SerializeField] public BubbleColor Color { get; private set; }
        
        public BubbleData(BubbleType type, BubbleColor color)
        {
            Type = type;
            Color = color;
        }
    }
}