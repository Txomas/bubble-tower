using System;
using Game.Saving;
using UnityEngine;
using Zenject;

namespace Game.Core
{
    [Serializable]
    public class PlayerStatsModel : BaseSingletonSavableModel
    {
        [Inject] private readonly SignalBus _signalBus;
        
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public int Coins { get; private set; }
        [field: SerializeField] public int Hearts { get; private set; }
        
        public void NextLevel()
        {
            Level++;
            _signalBus.Fire(new LevelChanged());
        }
        
        public void AddCoins(int amount)
        {
            Coins = Mathf.Max(0, Coins + amount);
            _signalBus.Fire(new CoinsChanged());
        }

        public void FailLevel()
        {
            Hearts = Mathf.Max(0, Hearts - 1);
            _signalBus.Fire(new HeartsChanged());
        }
    }
    
    public struct LevelChanged{}
    public struct CoinsChanged{}
    public struct HeartsChanged{}
}