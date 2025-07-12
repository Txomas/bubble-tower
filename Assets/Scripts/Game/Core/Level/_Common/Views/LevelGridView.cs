using UnityEngine;

namespace Game.Core.Level
{
    public class LevelGridView : MonoBehaviour
    {
        [SerializeField] private Transform _cellsContainer;
        [SerializeField] private Transform _playerBubbleAnchor;

        public Transform CellsContainer => _cellsContainer;
        public Transform PlayerBubbleAnchor => _playerBubbleAnchor;
    }
}