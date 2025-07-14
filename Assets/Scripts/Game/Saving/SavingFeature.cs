using Zenject.Helpers;

namespace Game.Saving
{
    public class SavingFeature : BaseFeatureInstaller<SavingFeature>
    {
        protected override void OnFeatureEnabled()
        {
            BindSingleton<SavingService>();
        }
    }
}