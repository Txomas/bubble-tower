using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Zenject.Helpers;

namespace Game.Input
{
    public class InputController : BaseController
    {
        [Inject] private InputSystemActions _actions;
        
        private Vector2 _lastScreenPos = Vector2.negativeInfinity;

        protected override void OnInitialized()
        {
            _actions.Gameplay.Position.performed += OnPositionChanged;
            _actions.Enable();
        }

        protected override void OnDisposed()
        {
            _actions.Gameplay.Position.performed -= OnPositionChanged;
            _actions.Disable();
        }

        private void OnPositionChanged(InputAction.CallbackContext ctx)
        {
            if (_actions.Gameplay.Click.phase is not InputActionPhase.Performed)
            {
                return;
            }
            
            var screenPos = _actions.Gameplay.Position.ReadValue<Vector2>();
            
            if (screenPos == _lastScreenPos)
            {
                return;
            }
            
            _lastScreenPos = screenPos;

            FireSignal(new PointerMovedSignal(screenPos));
        }
    }
}