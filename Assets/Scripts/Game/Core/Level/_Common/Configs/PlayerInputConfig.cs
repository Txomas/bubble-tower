using UnityEngine;

namespace Game.Core.Level
{
    [CreateAssetMenu(fileName = nameof(PlayerInputConfig), menuName = ConfigsPaths.Gameplay + nameof(PlayerInputConfig))]
    public class PlayerInputConfig : ScriptableObject
    {
        [Tooltip("Degrees per every unit of swipe")]
        public float TowerRotationSpeed = 100f;
    }
}