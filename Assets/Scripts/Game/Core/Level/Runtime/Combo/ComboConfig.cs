using System.Collections.Generic;
using System.Linq;
using Game.Core.Bubbles;
using UnityEngine;

namespace Game.Core.Level.Combo
{
    [CreateAssetMenu(fileName = nameof(ComboConfig), menuName = ConfigsPaths.Game + nameof(ComboConfig))]
    public class ComboConfig : ScriptableObject
    {
        [SerializeField] private float _comboResetTime = 1f;
        [SerializeField] private int _bonusBubbleEveryX = 5;
        [SerializeField] private BubbleType _bonusBubbleType;
        
        public bool ShouldResetCombo(float timeSinceLastBubble)
        {
            return timeSinceLastBubble > _comboResetTime;
        }
        
        public float GetResetPercent(float timeSinceLastBubble)
        {
            return Mathf.Clamp01(timeSinceLastBubble / _comboResetTime);
        }
        
        public BubbleType GetBubbleType(int streak)
        {
            return streak % _bonusBubbleEveryX != 0 ? BubbleType.Default : _bonusBubbleType;
        }
        
        [Space]
        [SerializeField] private int _changeColorStreak = 3;
        [SerializeField] private List<Color> _streakColors;
        
        public Color GetStreakColor(int streak)
        {
            var value = (float) streak / _changeColorStreak;
            var index = Mathf.FloorToInt(value);
            
            if (index >= _streakColors.Count - 1)
            {
                return _streakColors.Last();
            }
            
            var lerp = value - index;
            return Color.Lerp(_streakColors[index], _streakColors[index + 1], lerp);
        }
    }
}