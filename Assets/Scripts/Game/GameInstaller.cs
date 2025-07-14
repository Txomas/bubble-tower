using Game.Ads;
using Game.Core;
using Game.Input;
using Game.Popups;
using Game.Saving;
using Zenject.Helpers;

namespace Game
{
    public class GameInstaller : BaseMonoInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            AdsFeature.Install(Container);
            InputFeature.Install(Container);
            PopupsFeature.Install(Container);
            SavingFeature.Install(Container);
            
            GameCoreFeature.Install(Container);
        }
    }
}