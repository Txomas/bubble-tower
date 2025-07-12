using Game.Core.Level.Runtime;
using Zenject.Helpers;

namespace Game.Core
{
    public class GameCoreFeature : BaseFeatureInstaller<GameCoreFeature>
    {
        protected override void OnFeatureEnabled()
        {
            BindSingleton<PlayerProgressModel>();
            
            RuntimeLevelFeature.Install(Container);
        }
    }
}