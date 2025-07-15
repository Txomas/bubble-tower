using System;
using DG.Tweening;
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
        private void Awake()
        {
            DOTween.SetTweensCapacity(700, 50);
        }

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