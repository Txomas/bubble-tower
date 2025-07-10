using UnityEngine;

namespace Game.Core.Level
{
    [CreateAssetMenu(fileName = nameof(PlayerInputConfig), menuName = ConfigsPaths.Game + nameof(PlayerInputConfig))]
    public class PlayerInputConfig : ScriptableObject
    {
        public float TowerRotationSpeed = 100f; // degrees per unit
    }
}