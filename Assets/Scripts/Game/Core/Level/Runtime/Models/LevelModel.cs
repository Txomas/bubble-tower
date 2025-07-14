using Game.Core.Bubbles;
using UnityEngine;

namespace Game.Core.Level.Runtime
{
    public class LevelModel : BaseLevelModel
    {
        public int PlayersBubblesLeft { get; private set; }
        public LevelState State { get; private set; }

        public override void SetData(LevelData data)
        {
            base.SetData(data);
            
            PlayersBubblesLeft = data.PlayersBubblesCount;
        }

        public void UsePlayerBubble()
        {
            PlayersBubblesLeft--;
            _signalBus.Fire(new PlayerBubblesChanged());
        }
        
        public void SetState(LevelState state)
        {
            if (State == state)
            {
                return;
            }
            
            State = state;
            _signalBus.Fire(new LevelStateChanged(state));
        }

        public void RemoveBubble(Vector2Int index, bool isDropped)
        {
            ChangeBubbleColor(index, BubbleColor.None, isDropped);
        }
    }
}