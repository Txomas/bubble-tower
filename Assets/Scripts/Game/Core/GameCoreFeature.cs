using Game.Core.Level.Runtime;
using Game.Core.MainFlow;
using Zenject.Helpers;

namespace Game.Core
{
    public class GameCoreFeature : BaseFeatureInstaller<GameCoreFeature>
    {
        protected override void OnFeatureEnabled()
        {
            BindSingletonAndInterfaces<PlayerStatsModel>();

            DeclareSignal<LevelChanged>();
            DeclareOptionalSignal<CoinsChanged>();
            DeclareOptionalSignal<HeartsChanged>();
            
            RuntimeLevelFeature.Install(Container);
            MainFlowFeature.Install(Container);
        }
    }
}