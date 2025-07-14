using Game.Input;
using UnityEngine;
using Zenject;
using Zenject.Helpers;

namespace Game.Core.Level
{
    public class TowerController : BaseController
    {
        [Inject] private readonly IGridService _gridService;
        [Inject] private readonly LevelGridView _gridView;
        [Inject] private readonly PlayerInputConfig _inputConfig;
        [Inject] private readonly LevelGridConfig _gridConfig;

        protected override void OnInitialized()
        {
            var diameter = _gridConfig.Radius * 2f;
            var height = _gridConfig.Rows * _gridConfig.HeightStep;
            _gridView.Tower.localScale = new Vector3(diameter, height, diameter);
            _gridView.Tower.position = new Vector3(0f, -height / 2f, 0f);
        }

        protected override void OnEnabled()
        {
            Subscribe<PointerMovedSignal>(OnPointerMoved);
        }

        protected override void OnDisabled()
        {
            _gridView.CellsContainer.rotation = Quaternion.identity;
            UnsubscribeAll();
        }

        private void OnPointerMoved(PointerMovedSignal signal)
        {
            var angle = -signal.Delta.x * _inputConfig.TowerRotationSpeed;
            _gridView.CellsContainer.Rotate(Vector3.up, angle);
        }
    }
}