using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game.Core.Level.LevelEditor
{
    public class LevelDataService
    {
        public LevelData CreateNewLevelData()
        {
            return ScriptableObject.CreateInstance<LevelData>();
        }


        public string SaveLevelData(LevelData levelData)
        {
            if (!AssetDatabase.IsValidFolder(ConfigsPaths.LevelsFolder))
            {
                AssetDatabase.CreateFolder(ConfigsPaths.Assets + ConfigsPaths.Game.TrimEnd('/'), nameof(ConfigsPaths.Levels));
            }

            var levelName = "Level_" + GetAvailableLevelNames().Count.ToString("D2");
            
            SaveLevelData(levelName, levelData);
            
            return levelName;
        }

        public void SaveLevelData(string levelName, LevelData levelData)
        {
            var assetPath = GetLevelAssetPath(levelName);
            
            AssetDatabase.CreateAsset(levelData, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"Level data saved to: {assetPath}");
        }

        public LevelData LoadLevelData(string levelName)
        {
            var assetPath = GetLevelAssetPath(levelName);
            var levelData = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);
            
            if (levelData == null)
            {
                Debug.LogWarning($"Level data not found at: {assetPath}");
                return null;
            }
            
            return levelData;
        }
        
        private string GetLevelAssetPath(string levelName)
        {
            return ConfigsPaths.LevelsFolder + levelName + ConfigsPaths.AssetExtension;
        }

        public List<string> GetAvailableLevelNames()
        {
            if (!AssetDatabase.IsValidFolder(ConfigsPaths.LevelsFolder))
            {
                return new List<string>();
            }

            var guids = AssetDatabase.FindAssets("t:LevelData", new[] { ConfigsPaths.LevelsFolder });

            return guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }
    }
} 