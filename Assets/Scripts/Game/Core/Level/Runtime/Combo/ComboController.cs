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
            Subscribe<ShotFinished>(OnShoot);
            _view.gameObject.SetActive(false);
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
                _view.gameObject.SetActive(false);
            }
            else
            {
                var lerpValue = _config.GetResetPercent(timeSinceLastShoot);
                var color = _config.GetStreakColor(_model.Streak);
                color.a = 1 - lerpValue;
                _view.SetTextColor(color);
            }
        }

        private void OnShoot(ShotFinished signal)
        {
            if (signal.IsSuccessful)
            {
                // TODO: better to create new effect every time
                _model.IncrementStreak();
                _lastShootTime = Time.time;
                _view.SetCurrentCombo(_model.Streak);
                _view.gameObject.SetActive(true);
            }
            else
            {
                _model.ResetStreak();
                _view.gameObject.SetActive(false);
            }
        }
    }
}