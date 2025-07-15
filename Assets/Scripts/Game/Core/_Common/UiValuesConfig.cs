using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = nameof(UiValuesConfig), menuName = ConfigsPaths.Gameplay + nameof(UiValuesConfig))]
    public class UiValuesConfig : ScriptableObject
    {
        public float CounterTweenDuration = 0.5f;
    }
}