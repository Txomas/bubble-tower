using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Components
{
    public class TextButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        
        public Button.ButtonClickedEvent OnClick => _button.onClick;
        
        public bool Interactable
        {
            get => _button.interactable;
            set => _button.interactable = value;
        }
        
        public void SetText(string text)
        {
            if (_text != null)
            {
                _text.text = text;
            }
        }
    }
}