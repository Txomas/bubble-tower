#if UNITY_EDITOR
using System.Linq;
using Common.Utils;
using UnityEditor;

namespace Common
{
    public class ConfigsContainerAssetsPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets
        (
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths
        )
        {
            var configsContainer = AssetsTools.GetConfigInstance<ConfigsContainer>();

            if (configsContainer == null || configsContainer.FoldersPaths == null)
            {
                return;
            }
            
            var foldersPaths = configsContainer.FoldersPaths;

            var allChangedAssets = importedAssets
                .Concat(deletedAssets)
                .Concat(movedAssets)
                .Concat(movedFromAssetPaths)
                .Distinct()
                .Where(assetPath => assetPath.EndsWith(".asset"))
                .Where(path => foldersPaths.Contains(path[..path.LastIndexOf('/')]));
            
            if (allChangedAssets.Any())
            {
                configsContainer.UpdateFoldersConfigs();
            }
        }
    }
}
#endif