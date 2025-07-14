using System;
using UnityEngine;

namespace Game.Popups
{
    public interface IPopupController
    {
        event Action<PopupType> OnHidden;
        
        bool IsPopupActive { get; }
        bool IsReplaced { get; set; }
        PopupType Type { get; }
        Transform ViewTransform { get; }
        BasePopupView View { get; }
        object Data { get; }
        
        void Init(BasePopupView view, PopupType type);

        void HidePopup();
        void HidePopupImmediate(bool isSilent = false);

        bool TrySetData(object data);
        void ReassignData();
        void ShowPopup();
    }
}