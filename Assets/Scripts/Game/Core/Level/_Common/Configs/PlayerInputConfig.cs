using UnityEngine;

namespace Game.Core.Level
{
    [CreateAssetMenu(fileName = nameof(PlayerInputConfig), menuName = ConfigsPaths.Game + nameof(PlayerInputConfig))]
    public class PlayerInputConfig : ScriptableObject
    {
        [Tooltip("Degrees per every unit of swipe")]
        public float TowerRotationSpeed = 100f;
    }
}