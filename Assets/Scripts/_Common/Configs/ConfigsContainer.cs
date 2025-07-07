using System.Collections.Generic;
using Common.Attributes;
using UnityEngine;

namespace Common
{
    [CreateAssetMenu(fileName = nameof(ConfigsContainer), menuName = "Configs/" + nameof(ConfigsContainer))]
    public partial class ConfigsContainer : ScriptableObject
    {
        [ReadOnly] [SerializeField] private List<ScriptableObject> _configsInFolders;

        public IEnumerable<ScriptableObject> GetConfigs()
        {
            return _configsInFolders;
        }
    }
}