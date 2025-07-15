using System;
using System.Collections;
using Game.Popups;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Zenject.Helpers;

namespace Game.Ads
{
    public class AdsButtonController : BaseController
    {
        public event Action Success;

        [Inject] private readonly IAdVideoService _adVideoService;
        [Inject] private readonly IPopupsManager _popupsManager;
        
        private readonly Button _view;
        private readonly AdPlacement _placement;

        public AdsButtonController(Button view, AdPlacement placement)
        {
            _view = view;
            _placement = placement;

            _view.onClick.AddListener(OnButtonClicked);
        }

        protected override void OnDisposed()
        {
            _view.onClick.RemoveListener(OnButtonClicked);
            Success = null;
        }

        private void OnButtonClicked()
        {
            StartCoroutine(ShowVideoCoroutine());
        }

        private IEnumerator ShowVideoCoroutine()
        {
            if (!_adVideoService.IsVideoReady)
            {
                if (!_adVideoService.IsVideoRequested)
                {
                    _adVideoService.LoadVideo();
                }
                
                _popupsManager.ShowPopup(PopupType.AdLoader, PopupMode.Additive);
                
                yield return new WaitWhile(() => _popupsManager.IsPopupActive(PopupType.AdLoader));
            }
            
            if (_adVideoService.IsVideoReady)
            {
                _adVideoService.ShowVideo(_placement, isSuccess =>
                {
                    if (isSuccess)
                    {
                        Success?.Invoke();
                    }
                });	
            }
        }
    }
}