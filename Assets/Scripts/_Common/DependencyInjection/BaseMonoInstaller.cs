using Common;
using UnityEngine;

namespace Zenject.Helpers
{
    public abstract class BaseMonoInstaller : MonoInstaller
    {
        [SerializeField] private ConfigsContainer _configsContainer;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            var configs = _configsContainer.GetConfigs();
            
            foreach (var config in configs)
            {
                Container.Bind(config.GetType())
                    .FromInstance(config)
                    .AsSingle()
                    .NonLazy();
            }
        }
    }
}