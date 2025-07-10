using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Level
{
    [CreateAssetMenu(fileName = nameof(LevelsConfig), menuName = ConfigsPaths.Game + nameof(LevelsConfig))]
    public class LevelsConfig : ScriptableObject
    {
        [SerializeField] private List<LevelData> _levels;
    }
}