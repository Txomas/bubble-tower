using System.Collections.Generic;
using UnityEngine;
using Common.Utils;

namespace Game.Saving
{
    public class SavingService
    {
        private readonly Dictionary<object, string> _keysByObjects = new();
        private readonly Dictionary<SavingCategory, List<object>> _objectsByCategory = new();
        private readonly HashSet<object> _savingBlockers = new();
        
        public bool IsSavingEnabled => _savingBlockers.Count == 0;
        public float LastSaveAllTime { get; private set; }
        
        /// <param name="obj">object to save</param>
        /// <param name="key">save key</param>
        /// <param name="category">category for feature saves reset</param>
        public void AddSavableObject(object obj, string key, SavingCategory category = SavingCategory.Undefined)
        {
            // should load for correct prestige transition
            var jsonData = PlayerPrefs.GetString(key);
            StorageUtil.FromJsonOverwrite(obj, jsonData);
            
            _keysByObjects.TryAdd(obj, key);
            _objectsByCategory.TryAdd(category, new List<object>());
            _objectsByCategory[category].Add(obj);
        }
        
        public void DeleteSaves(SavingCategory category)
        {
            if (_objectsByCategory.TryGetValue(category, out var objects))
            {
                foreach (var obj in objects)
                {
                    if (_keysByObjects.TryGetValue(obj, out var key))
                    {
                        PlayerPrefs.DeleteKey(key);
                        _keysByObjects.Remove(obj);
                    }
                }
                
                _objectsByCategory.Remove(category);
            }
        }
        
        /// <summary>
        /// Heavy operation, saves all objects in the service.
        /// Use carefully, as it can cause performance issues if called too frequently.
        /// </summary>
        public void SaveAll()
        {
            foreach (var obj in _keysByObjects.Keys)
            {
                Save(obj);
            }
            
            LastSaveAllTime = Time.time;
            PlayerPrefs.Save();
        }

        public void Save(object obj)
        {
            if (IsSavingEnabled && _keysByObjects.TryGetValue(obj, out var key))
            {
                StorageUtil.SaveData(obj, key, shouldSave: false);
            }
        }
        
        /// <summary>
        /// Saves the object immediately even if saving is blocked.
        /// </summary>
        public void ForceSave(object obj)
        {
            StorageUtil.SaveData(obj, _keysByObjects[obj]);
        }
        
        /// <param name="blocker">Blocking object/key</param>
        /// <param name="shouldSave">Should SaveAll before blocking</param>
        public void BlockSaving(object blocker, bool shouldSave = false)
        {
            if (shouldSave)
            {
                SaveAll();
            }
            
            _savingBlockers.Add(blocker);
        }
        
        /// <param name="blocker">Blocking object/key - should be the same as in <see cref="BlockSaving"/> call</param>
        /// <param name="shouldTrySave">Should try SaveAll after unblock</param>
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