using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Level
{
    [CreateAssetMenu(fileName = nameof(LevelsConfig), menuName = ConfigsPaths.Gameplay + nameof(LevelsConfig))]
    public class LevelsConfig : ScriptableObject
    {
        [SerializeField] private List<LevelData> _levels;
        
        public int LevelsCount => _levels.Count;
        
        public LevelData GetLevelData(int level)
        {
            if (level < 0 || level >= _levels.Count)
            {
                Debug.LogError($"{level} is out of range. Valid range: 0 - {_levels.Count - 1}", this);
                return _levels[0];
            }
            
            return _levels[level];
        }
    }
}