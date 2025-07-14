namespace Game.Core
{
    public interface ILevelManager
    {
        void StartLevel();
        void RestartLevel();
        void CompleteLevel();
        void FailLevel();
    }
}