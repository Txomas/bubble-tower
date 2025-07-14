using System;
using System.IO;
using UnityEngine;

namespace Common.Utils
{
    public static class StorageUtil
    {
        public static bool HasDataForObject(object data)
        {
            return HasData(data.GetType().Name);
        }
        
        public static bool HasData(string key)
        {
        	return PlayerPrefs.HasKey(key);
        }
        
        public static void SaveData(object data)
        {
            SaveData(data, data.GetType().Name);
        }

        public static void SaveData(object data, string key, bool isEditorPrefs = false, bool shouldSave = true)
        {
            var jsonData = JsonUtility.ToJson(data);
            
            if (isEditorPrefs)
            {
                #if UNITY_EDITOR
                UnityEditor.EditorPrefs.SetString(key, jsonData);
                #endif
            }
            else
            {
                PlayerPrefs.SetString(key, jsonData);
                
                if (shouldSave)
                {
                    PlayerPrefs.Save();
                }
            }
        }

        public static void LoadData(object data)
        {
            var jsonData = PlayerPrefs.GetString(data.GetType().Name);
            FromJsonOverwrite(data, jsonData);
        }
        
        public static void FromJsonOverwrite(object obj, string jsonData)
        {
            if (!string.IsNullOrEmpty(jsonData))
            {
                try
                {
                    JsonUtility.FromJsonOverwrite(jsonData, obj);
                }
                catch (Exception exception)
                {
                    Debug.LogWarning($"Can't load data for \"{obj.GetType().Name}\": {exception.Message}");
                }
            }
        }
    }
}