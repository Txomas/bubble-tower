using UnityEngine;

namespace Game.Core.Level
{
    public class LevelGridView : MonoBehaviour
    {
        [SerializeField] private Transform _cellsContainer;

        public Transform CellsContainer => _cellsContainer;
    }
}