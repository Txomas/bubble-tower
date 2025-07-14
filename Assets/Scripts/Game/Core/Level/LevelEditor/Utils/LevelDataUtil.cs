#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.Core.Bubbles;
using UnityEditor;
using UnityEngine;

namespace Game.Core.Level.LevelEditor
{
    public static class LevelDataUtil
    {
        public static string SaveNewLevelData(IReadOnlyDictionary<Vector2Int, BubbleData> data)
        {
            if (!AssetDatabase.IsValidFolder(ConfigsPaths.LevelsFolder))
            {
                AssetDatabase.CreateFolder(ConfigsPaths.Assets + ConfigsPaths.Game.TrimEnd('/'), nameof(ConfigsPaths.Levels));
            }

            var levelName = "Level_" + GetAvailableLevelNames().Count.ToString("D2");
            
            var assetPath = GetLevelAssetPath(levelName);
            
            var levelData = ScriptableObject.CreateInstance<LevelData>();
            levelData.SetBubbles(data);
            
            AssetDatabase.CreateAsset(levelData, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"Level data saved to: {assetPath}");
            
            return levelName;
        }

        public static void SaveLevelData(string levelName, IReadOnlyDictionary<Vector2Int, BubbleData> levelData)
        {
            var data = LoadLevelData(levelName);
            
            if (data == null)
            {
                Debug.LogWarning($"Level data not found for: {levelName}");
                return;
            }
            
            data.SetBubbles(levelData);
        }

        public static LevelData LoadLevelData(string levelName)
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
        
        private static string GetLevelAssetPath(string levelName)
        {
            return ConfigsPaths.LevelsFolder + levelName + ConfigsPaths.AssetExtension;
        }

        private static List<string> GetAvailableLevelNames()
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
#endif