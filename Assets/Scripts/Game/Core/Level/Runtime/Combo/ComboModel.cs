namespace Game.Core.Level.Runtime.Combo
{
    public class ComboModel
    {
        public int Streak { get; private set; }
        
        public void ResetStreak()
        {
            Streak = 0;
        }
        
        public void IncrementStreak()
        {
            Streak++;
        }
    }
}