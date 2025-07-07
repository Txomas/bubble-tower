using UnityEngine;

namespace Game
{
    public class LevelGridView : MonoBehaviour
    {
        [SerializeField] private Transform _cellsContainer;

        public Transform CellsContainer => _cellsContainer;
    }
}