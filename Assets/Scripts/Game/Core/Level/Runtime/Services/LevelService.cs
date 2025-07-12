using UnityEngine;
using Zenject;

namespace Game.Core.Level.Runtime
{
    public class LevelService
    {
        [Inject] private readonly LevelsConfig _levelsConfig;
        [Inject] private readonly PlayerProgressModel _playerProgressModel;
        
        public LevelData GetCurrentLevelData()
        {
            var level = _playerProgressModel.Level;
            
            if (level < 0)
            {
                Debug.LogError($"Invalid level index: {level}. Level index must be non-negative.");
                return _levelsConfig.GetLevelData(0);
            }
            
            if (level >= _levelsConfig.LevelsCount)
            {
                level = Random.Range(0, _levelsConfig.LevelsCount);
            }
            
            return _levelsConfig.GetLevelData(level);
        }
    }
}