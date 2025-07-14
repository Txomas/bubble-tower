using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Game.Core.Bubbles;
using UnityEngine;

namespace Game.Core.Level
{
    public class LevelData : ScriptableObject
    {
        public int PlayersBubblesCount;
        
        [SerializeField] private SerializedDictionary<Vector2Int, BubbleData> _bubbles = new();

        public IReadOnlyDictionary<Vector2Int, BubbleData> Bubbles => _bubbles;

#if UNITY_EDITOR
        public void SetBubbles(IReadOnlyDictionary<Vector2Int, BubbleData> bubbles)
        {
            _bubbles = new SerializedDictionary<Vector2Int, BubbleData>(bubbles);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}