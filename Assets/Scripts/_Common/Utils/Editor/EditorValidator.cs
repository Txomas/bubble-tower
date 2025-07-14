using UnityEditor;
using UnityEngine;

namespace Common.Utils
{
    public static class EditorValidator
    {
        [MenuItem(MenuItemPaths.Tools + "Validate all loaded objects")]
        private static void ValidateScene()
        {
            var objectsToValidate = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            
            var isSuccessful = Validator.ValidateObjects(objectsToValidate);
            
            if (isSuccessful)
            {
                Debug.Log("All objects fields are valid");
            }
        }
        
        [InitializeOnEnterPlayMode]
        [MenuItem(MenuItemPaths.Tools + "Validate configs")]
        public static void ValidateConfigs()
        {
            var objectsToValidate = AssetsTools.GetConfigInstance<ConfigsContainer>().Configs;
            
            var isSuccessful = Validator.ValidateObjects(objectsToValidate);
            
            if (isSuccessful)
            {
                Debug.Log("All configs fields are valid");
            }
        }
    }
}