using System;
using Game.Core.Bubbles;
using UnityEngine;

namespace Game.Core.Level
{
    public class LevelGridView : MonoBehaviour
    {
        public event Action<BubbleView> BubbleAdded;
        
        [SerializeField] private Transform _cellsContainer;
        [SerializeField] private Transform _playerBubbleContainer;
        [SerializeField] private ParticleSystem _shootEffect;
        [SerializeField] private Transform _tower;

        public Transform CellsContainer => _cellsContainer;
        public Transform PlayerBubbleContainer => _playerBubbleContainer;
        public Transform Tower => _tower;
        
        public void AddBubble(BubbleView bubble, Vector2Int index)
        {
            bubble.GridIndex = index;
            bubble.transform.SetParent(_cellsContainer, true);
            BubbleAdded?.Invoke(bubble);
        }
        
        public void PlayShootEffect()
        {
            _shootEffect.Play();
        }
    }
}