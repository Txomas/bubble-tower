using TMPro;
using UnityEngine;

namespace Game.Core.Level.Runtime
{
    public class LevelInterfaceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _bubblesCounter;
        
        public void SetBubblesCount(int count)
        {
            _bubblesCounter.text = count.ToString();
        }
    }
}