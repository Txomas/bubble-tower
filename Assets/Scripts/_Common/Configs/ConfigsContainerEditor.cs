#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Common.Attributes;
using UnityEditor;
using UnityEngine;

namespace Common
{
    public partial class ConfigsContainer
    {
        [CallOnChange(nameof(UpdateFoldersConfigs))]
        [SerializeField] private List<DefaultAsset> _folders;
        [HideInInspector] [SerializeField] private string[] _foldersPaths;
        
        public IReadOnlyList<string> FoldersPaths => _foldersPaths;
        
        private void UpdateFoldersPaths()
        {
            _foldersPaths = _folders
                .Where(folder => folder != null)
                .Distinct()
                .Select(AssetDatabase.GetAssetPath)
                .ToArray();
        }
        
        private void OnValidate()
        {
            if (_folders.Count != _foldersPaths.Length)
            {
                UpdateFoldersConfigs();
            }
        }

        [ContextMenu(nameof(UpdateFoldersConfigs))]
        public void UpdateFoldersConfigs()
        {
            _folders.RemoveAll(folder => !AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(folder)));
            
            UpdateFoldersPaths();
            
            _configsInFolders = AssetDatabase.FindAssets("t:ScriptableObject", _foldersPaths)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => _foldersPaths.Contains(path[..path.LastIndexOf('/')]))
                .Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>)
                .Where(scriptableObject => scriptableObject != null)
                .ToList();
            
            EditorUtility.SetDirty(this);
        }
    }
}
#endif