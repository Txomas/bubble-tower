using Game.Core.Level.Runtime;
using Game.Core.MainFlow;
using Zenject.Helpers;

namespace Game.Core
{
    public class GameCoreFeature : BaseFeatureInstaller<GameCoreFeature>
    {
        protected override void OnFeatureEnabled()
        {
            BindSingleton<PlayerStatsModel>();

            DeclareSignal<LevelChanged>();
            DeclareSignal<CoinsChanged>();
            DeclareSignal<HeartsChanged>();
            
            RuntimeLevelFeature.Install(Container);
            MainFlowFeature.Install(Container);
        }
    }
}