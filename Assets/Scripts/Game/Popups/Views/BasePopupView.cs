using System;
using System.Collections.Generic;
using Common.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Popups
{
    public abstract class BasePopupView : VisibilityAnimator
    {
        public event Action CloseClicked;

        [SerializeField] private CanvasGroup _interactableCanvasGroup;
        [SerializeField] private List<Button> _closeButtons;

        protected virtual void Awake()
        {
            _closeButtons.ForEach(button => button.onClick.AddListener(() => CloseClicked?.Invoke()));
        }
        
        public void SetIsInteractable(bool isInteractable)
        {
            if (_interactableCanvasGroup != null)
            {
                _interactableCanvasGroup.interactable = isInteractable;
            }
        }
    }
}