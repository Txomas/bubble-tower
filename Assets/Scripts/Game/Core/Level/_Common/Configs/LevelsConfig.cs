using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = nameof(LevelsConfig), menuName = ConfigsPaths.Configs + nameof(LevelsConfig))]
    public class LevelsConfig : ScriptableObject
    {
        [SerializeField] private List<LevelData> _levels;
    }
}