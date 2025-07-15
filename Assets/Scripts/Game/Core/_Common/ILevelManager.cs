namespace Game.Core
{
    public interface ILevelManager
    {
        void StartLevel();
        void CompleteLevel();
        void FailLevel();
    }
}