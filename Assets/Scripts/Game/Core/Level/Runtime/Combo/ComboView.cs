using TMPro;
using UnityEngine;

namespace Game.Core.Level.Runtime.Combo
{
    public class ComboView : MonoBehaviour
    {
        private const string ComboTextFormat = "x{0}";
        
        [SerializeField] private TMP_Text _comboText;
        
        public void SetCurrentCombo(int comboCount)
        {
            _comboText.text = string.Format(ComboTextFormat, comboCount);
        }
        
        public void SetTextColor(Color color)
        {
            _comboText.color = color;
        }
    }
}