using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Zenject.Helpers;
using Object = UnityEngine.Object;

namespace Game.Popups
{
    public class PopupsManager : BaseController, IPopupsManager
    {
        [Inject] private readonly PopupsRootView _rootView;
        [Inject] private readonly PopupsPrefabsRefs _prefabsRefs;
        
        private readonly Dictionary<PopupType, IPopupController> _controllersByType = new();
        private readonly LinkedList<IPopupController> _popupsQueue = new();
        private readonly Dictionary<PopupType, Queue<object>> _queuedDataByType = new();

        private WaitUntil _waitUntilEmpty;
        public WaitUntil WaitUntilEmpty => _waitUntilEmpty ??= new WaitUntil(IsEmpty);

        public void HidePopups()
        {
            foreach (var controller in _popupsQueue)
            {
                controller.HidePopup();
            }
        }

        public void ClearQueue()
        {
            foreach (var controller in _popupsQueue)
            {
                controller.HidePopupImmediate(true);
            }

            _popupsQueue.Clear();
        }
        
        public bool IsPopupActive(PopupType popupType)
        {
            return GetActivePopupController(popupType) != null;
        }
        
        public IPopupController GetActivePopupController(PopupType popupType)
        {
            return _popupsQueue.FirstOrDefault(controller => controller.Type == popupType);
        }

        private void OnPopupHidden(PopupType type)
        {
            var controller = GetPopupController(type);

            if (!controller.IsReplaced)
            {
                _popupsQueue.Remove(controller);
            }
            
            TryShowNextPopup();
        }

        private void TryShowNextPopup()
        {
            var controller = _popupsQueue.First?.Value;

            if (controller is { IsPopupActive: false })
            {
                if (controller.IsReplaced)
                {
                    controller.IsReplaced = false;
                    controller.ReassignData();
                }
                
                if (_queuedDataByType.TryGetValue(controller.Type, out var queue) && queue.Count > 0)
                {
                    controller.TrySetData(queue.Dequeue());
                }
                
                controller.ShowPopup();
            }
        }

        public void ShowPopup(PopupType type, PopupMode mode = PopupMode.Queued, object data = null, bool isMultiple = false)
        {
            if (!TryRequestPopupController(type, out var controller, data))
            {
                return;
            }
            
            if (!isMultiple && _popupsQueue.Contains(controller))
            {
                Debug.LogWarning($"Trying to show \"{type.ToString()}\" popup twice.");
                return;
            }
            
            if (mode is PopupMode.Queued)
            {
                if (isMultiple && _popupsQueue.Count >= 1)
                {
                    _queuedDataByType.TryAdd(type, new Queue<object>());
                    _queuedDataByType[type].Enqueue(data);
                }

                _popupsQueue.AddLast(controller);

                if (_popupsQueue.Count == 1)
                {
                    controller.ShowPopup();
                }
            }
            else
            {
                if (mode is PopupMode.Replace)
                {
                    var popup = _popupsQueue.First?.Value;
                    
                    if (popup != null)
                    {
                        popup.IsReplaced = true;
                        popup.HidePopup();
                    }
                }

                _popupsQueue.AddFirst(controller);
                controller.ShowPopup();
                controller.ViewTransform.SetAsLastSibling();
            }
        }

        private bool TryRequestPopupController(PopupType type, out IPopupController controller, object data = null)
        {
            controller = GetPopupController(type);

            return controller != null && controller.TrySetData(data);
        }

        private IPopupController GetPopupController(PopupType type)
        {
            return _controllersByType.TryGetValue(type, out var value) ? value : CreatePopupController(type);
        }

        private IPopupController CreatePopupController(PopupType type)
        {
            if (TryCreateControllerWithId<IPopupController>(type, out var controller))
            {
                controller.OnHidden += OnPopupHidden;

                var prefab = _prefabsRefs.GetPopupPrefab(type);
                var view = Object.Instantiate(prefab, _rootView.transform);
                controller.Init(view, type);

                _controllersByType.Add(type, controller);
                return controller;
            }
            else
            {
                Debug.LogError($"Popup with type \"{type.ToString()}\" is not registered");
                return null;
            }
        }

        public bool IsEmpty()
        {
            return _popupsQueue.Count == 0;
        }
    }
}