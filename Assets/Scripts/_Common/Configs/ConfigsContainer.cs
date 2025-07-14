using System.Collections.Generic;
using Common.Attributes;
using UnityEngine;
using Zenject;

namespace Common
{
    [CreateAssetMenu(fileName = nameof(ConfigsContainer), menuName = "Configs/" + nameof(ConfigsContainer))]
    public partial class ConfigsContainer : ScriptableObjectInstaller
    {
        [ReadOnly] [SerializeField] private List<ScriptableObject> _configsInFolders;
        
        public IEnumerable<ScriptableObject> Configs => _configsInFolders;
        
        public override void InstallBindings()
        {
            foreach (var config in Configs)
            {
                Container.Bind(config.GetType())
                    .FromInstance(config)
                    .AsSingle()
                    .NonLazy();
            }
        }
    }
}