using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Game.Core.Bubbles
{
    [CreateAssetMenu(fileName = nameof(BubblesConfig), menuName = ConfigsPaths.Configs + nameof(BubblesConfig))]
    public class BubblesConfig : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<BubbleColor, Color> _colors;

        public Color GetBubbleColor(BubbleColor color)
        {
            return _colors[color];
        }
    }
}