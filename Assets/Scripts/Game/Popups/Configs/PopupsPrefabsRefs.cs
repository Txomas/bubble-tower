using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Common.DataTypes;
using Common.Extensions;
using Common.Utils;
using UnityEngine;

namespace Game.Popups
{
    [CreateAssetMenu(fileName = nameof(PopupsPrefabsRefs), menuName = ConfigsPaths.Game + nameof(PopupsPrefabsRefs))]
    public class PopupsPrefabsRefs : ScriptableObject, IValidatable
    {
        [SerializeField] private SerializedDictionary<PopupType, BasePopupView> _popupsByType;

        public BasePopupView GetPopupPrefab(PopupType type)
        {
            return _popupsByType[type];
        }
        
        public bool IsValid(Object context, IAddOnlyList<string> messages)
        {
            if (EnumExtensions.GetEnumList<PopupType>()
                .All(type => _popupsByType.GetValueOrDefault(type) != null))
            {
                return true;
            }

            messages.Add("Not all popups prefabs are set in config");
            return false;
        }
    }
}