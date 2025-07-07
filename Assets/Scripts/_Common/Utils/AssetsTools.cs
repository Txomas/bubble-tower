using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Common.Utils
{
#if UNITY_EDITOR    
    public static class AssetsTools
    {
        [CanBeNull]
        public static T GetConfigInstance<T>() where T : ScriptableObject
        {
            return AssetDatabase
                .FindAssets("t:" + typeof(T).Name)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .FirstOrDefault(asset => asset != null);
        }

        [CanBeNull]
        public static T GetConfigOfType<T>()
        {
            return AssetDatabase
                .FindAssets("t:" + nameof(ScriptableObject))
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>)
                .OfType<T>()
                .FirstOrDefault();
        }
        
        public static IEnumerable<T> GetConfigsInstances<T>() where T : ScriptableObject
        {
            return AssetDatabase
                .FindAssets("t:" + typeof(T).Name)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .Where(asset => asset != null);
        }
    }
#else
    public static class AssetsTools
    {
        public static T GetConfigInstance<T>() where T : ScriptableObject
        {
            return default;
        }
        
        public static T GetConfigOfType<T>()
        {
            return default;
        }
        
        public static IEnumerable<T> GetConfigsInstances<T>() where T : ScriptableObject
        {
            return default;
        }
    }
#endif
}