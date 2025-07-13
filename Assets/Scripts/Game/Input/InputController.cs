using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;
using Zenject.Helpers;

namespace Game.Input
{
    public class InputController : BaseController
    {
        [Inject] private InputSystemActions _actions;
        private InputSystemActions.GameplayActions _gameplayActions;
        
        protected override void OnInitialized()
        {
            _gameplayActions = _actions.Gameplay;
            
            _gameplayActions.Scroll.performed += ctx =>
            {
                var delta = ctx.ReadValue<Vector2>().y;
                FireSignal(new ScrollSignal(delta));
            };
            _gameplayActions.Next.performed += _ =>
            {
                FireSignal(new NextClickedSignal());
            };
            _gameplayActions.Previous.performed += _ =>
            {
                FireSignal(new PreviousClickedSignal());
            };
            
            _actions.Enable();
        }

        protected override void OnTick()
        {
            if (_gameplayActions.Move.phase is InputActionPhase.Performed)
            {
                var direction = _gameplayActions.Move.ReadValue<Vector2>();
                FireSignal(new MoveSignal(direction));
            }
            
            if (_gameplayActions.Press.phase is InputActionPhase.Performed && 
                !EventSystem.current.IsPointerOverGameObject())
            {
                var screenPos = _gameplayActions.Position.ReadValue<Vector2>();
                var delta = _gameplayActions.Delta.ReadValue<Vector2>() * Time.deltaTime;
                FireSignal(new PointerMovedSignal(screenPos, delta));
            }
            
            if (_gameplayActions.Tap.WasPerformedThisFrame() && 
                !EventSystem.current.IsPointerOverGameObject())
            {
                var screenPos = _gameplayActions.Position.ReadValue<Vector2>();
                FireSignal(new PointerTappedSignal(screenPos));
            }
        }
    }
}