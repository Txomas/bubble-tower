using Zenject.Helpers;

namespace Game.Core.Level.Runtime.Combo
{
    public class ComboFeature : BaseFeatureInstaller<ComboFeature>
    {
        protected override void OnFeatureEnabled()
        {
            // TODO: better to dynamically instantiate View
            BindFromComponentInHierarchy<ComboView>();
            BindRootController<ComboController>();

            BindSingleton<ComboModel>();
            
            Container.Decorate<IPlayerBubbleDataProvider>().With<ComboPlayerBubbleDataProvider>();
        }

        // TODO: check for A/B
        public override bool IsEnabled => base.IsEnabled;
    }
}