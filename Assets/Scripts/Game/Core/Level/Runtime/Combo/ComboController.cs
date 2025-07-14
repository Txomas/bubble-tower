using Game.Core.Level.Combo;
using UnityEngine;
using Zenject;
using Zenject.Helpers;

namespace Game.Core.Level.Runtime.Combo
{
    public class ComboController : BaseController
    {
        [Inject] private readonly ComboView _view;
        [Inject] private readonly ComboModel _model;
        [Inject] private readonly ComboConfig _config;
        private float _lastShootTime;

        protected override void OnInitialized()
        {
            Subscribe<SuccessfullyShoot>(OnSuccessfullyShoot);
            _view.SetTextAlpha(0f);
        }

        protected override void OnTick()
        {
            if (_model.Streak <= 0)
            {
                return;
            }
            
            var timeSinceLastShoot = Time.time - _lastShootTime;
            
            if (_config.ShouldResetCombo(timeSinceLastShoot))
            {
                _model.ResetStreak();
                _view.SetTextAlpha(0f);
            }
            else
            {
                var lerpValue = _config.GetResetPercent(timeSinceLastShoot);
                _view.SetTextAlpha(1 - lerpValue);
                
                var color = _config.GetStreakColor(_model.Streak);
                _view.SetTextColor(color);
            }
        }

        private void OnSuccessfullyShoot(SuccessfullyShoot signal)
        {
            // TODO: better to create new effect every time
            _model.IncrementStreak();
            _lastShootTime = Time.time;
            _view.SetCurrentCombo(_model.Streak);
        }
    }
}