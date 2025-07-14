using Common.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.MainFlow
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private TextButton _startButton;
        
        public Button.ButtonClickedEvent StartButtonClicked => _startButton.OnClick;

        public void SetCurrentLevel(int level)
        {
            _startButton.SetText($"Level: {level + 1}");
        }
    }
}