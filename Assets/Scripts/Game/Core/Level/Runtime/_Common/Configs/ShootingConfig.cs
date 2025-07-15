using UnityEngine;

namespace Game.Core.Level.Runtime
{
    [CreateAssetMenu(fileName = nameof(ShootingConfig), menuName = ConfigsPaths.Gameplay + nameof(ShootingConfig))]
    public class ShootingConfig : ScriptableObject
    {
        [Tooltip("Speed of the bubble in units per second when shooting")]
        public float ShootSpeed = 5f;
        public int MinToPop = 3;
        public float BallsDropOffset = 10f;
    }
}