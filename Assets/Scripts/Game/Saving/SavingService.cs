using System.Collections.Generic;
using UnityEngine;
using Common.Utils;

namespace Game.Saving
{
    public class SavingService
    {
        private readonly Dictionary<object, string> _keysByObjects = new();
        private readonly HashSet<object> _savingBlockers = new();
        
        public bool IsSavingEnabled => _savingBlockers.Count == 0;
        
        public void AddSavableObject(object obj, string key)
        {
            var jsonData = PlayerPrefs.GetString(key);
            StorageUtil.FromJsonOverwrite(obj, jsonData);
            
            _keysByObjects.TryAdd(obj, key);
        }
        
        public void SaveAll()
        {
            foreach (var obj in _keysByObjects.Keys)
            {
                Save(obj);
            }
        }

        public void Save(object obj)
        {
            if (IsSavingEnabled && _keysByObjects.TryGetValue(obj, out var key))
            {
                StorageUtil.SaveData(obj, key, shouldSave: false);
            }
        }
        
        public void ForceSave(object obj)
        {
            StorageUtil.SaveData(obj, _keysByObjects[obj]);
        }
        
        public void BlockSaving(object blocker, bool shouldSave = false)
        {
            if (shouldSave)
            {
                SaveAll();
            }
            
            _savingBlockers.Add(blocker);
        }
        
        public void UnblockSaving(object blocker, bool shouldTrySave = true)
        {
            _savingBlockers.Remove(blocker);
            
            if (shouldTrySave)
            {
                SaveAll();
            }
        }
    }
}