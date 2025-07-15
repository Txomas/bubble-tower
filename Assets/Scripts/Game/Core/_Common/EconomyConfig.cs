using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = nameof(EconomyConfig), menuName = ConfigsPaths.Gameplay + nameof(EconomyConfig))]
    public class EconomyConfig : ScriptableObject
    {
        public int CoinsForWin = 50;
        public int AdsWinMultiplier = 3;
    }
}