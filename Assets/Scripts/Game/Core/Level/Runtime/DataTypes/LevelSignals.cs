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
}