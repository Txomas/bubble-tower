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

        public void RemovePlayerBubble()
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
            
            if (State == LevelState.Shooting)
            {
                _signalBus.Fire(new ShootingFinished());
            }
            
            State = state;
        }
    }
}