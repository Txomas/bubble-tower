namespace Game.Core.Level.Runtime
{
    public struct PlayerBubblesChanged
    {
    }
    
    public struct LevelStateChanged
    {
        public LevelState State { get; }

        public LevelStateChanged(LevelState state)
        {
            State = state;
        }
    }
    
    public struct ShotFinished
    {
        public bool IsSuccessful { get; }
        
        public ShotFinished(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
    }
    
    public struct LevelFinished
    {
    }
}