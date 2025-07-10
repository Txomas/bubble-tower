using Game.Input;
using UnityEngine;
using Zenject;
using Zenject.Helpers;

namespace Game.Core.Level
{
    public class TowerController : BaseController
    {
        [Inject] private readonly LevelGridView _gridView;
        [Inject] private readonly PlayerInputConfig _inputConfig;

        protected override void OnEnabled()
        {
            Subscribe<PointerMovedSignal>(OnPointerMoved);
        }

        protected override void OnDisabled()
        {
            UnsubscribeAll();
        }

        private void OnPointerMoved(PointerMovedSignal signal)
        {
            var angle = signal.Delta.x * _inputConfig.TowerRotationSpeed;
            _gridView.CellsContainer.Rotate(Vector3.up, angle);
        }
    }
}