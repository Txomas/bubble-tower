using Game.Popups;
using Zenject;

namespace Game.Core.MainFlow
{
    public class FailPopupController : BasePopupController<FailPopupView>
    {
        [Inject] private readonly ILevelManager _levelManager;

        protected override void OnPopupHidden()
        {
            _levelManager.FailLevel();
        }
    }
}