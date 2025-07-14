using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Game.Core.Bubbles
{
    [CreateAssetMenu(fileName = nameof(BubblesConfig), menuName = ConfigsPaths.Game + nameof(BubblesConfig))]
    public class BubblesConfig : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<BubbleType, BubbleView> _prefabs;
        [SerializeField] private List<BubbleType> _noColorTypes;
        [SerializeField] private SerializedDictionary<BubbleColor, Color> _colors;

        public BubbleView CreateBubble(BubbleData data, Transform root = null)
        {
            var view = Instantiate(_prefabs[data.Type], root);
            SetBubbleColor(view, data);
            return view;
        }
        
        public void SetBubbleColor(BubbleView bubble, BubbleData data)
        {
            if (!_noColorTypes.Contains(data.Type))
            {
                bubble.Color = _colors[data.Color];
            }
        }
    }
}