using UnityEngine;

namespace Game.Popups
{
	public interface IPopupsManager
	{
		WaitUntil WaitUntilEmpty { get; }
		
		void HidePopups();
		void ClearQueue();
		void ShowPopup(PopupType type, PopupMode mode = PopupMode.Queued, object data = null, bool isMultiple = false);
		
		bool IsPopupActive(PopupType popupType);
		
		bool IsEmpty();
	}
}