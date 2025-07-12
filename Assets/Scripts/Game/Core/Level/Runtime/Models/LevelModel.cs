namespace Game.Core.Level.Runtime
{
    public class LevelModel : BaseLevelModel
    {
        public int PlayersBubblesLeft { get; private set; }
        
        public void RemovePlayerBubble()
        {
            PlayersBubblesLeft--;
            _signalBus.Fire(new PlayerBubblesChanged());
        }
    }
}