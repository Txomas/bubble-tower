using Game.Core;
using Game.Input;
using Zenject.Helpers;

namespace Game
{
    public class GameInstaller : BaseMonoInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            InputFeature.Install(Container);
            GameCoreFeature.Install(Container);
        }
    }
}