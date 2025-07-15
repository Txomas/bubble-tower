using Common.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.MainFlow
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private TextButton _startButton;
        [SerializeField] private TMP_Text _coinsCounter;
        [SerializeField] private TMP_Text _heartsCounter;
        
        public Button.ButtonClickedEvent StartButtonClicked => _startButton.OnClick;
        public int Coins { get; private set; }

        public void SetCurrentLevel(int level)
        {
            _startButton.SetText($"Level: {level + 1}");
        }
        
        public void SetCoins(int coins)
        {
            Coins = coins;
            _coinsCounter.text = coins.ToString();
        }
        
        public void SetHearts(int hearts)
        {
            _heartsCounter.text = hearts.ToString();
        }
    }
}