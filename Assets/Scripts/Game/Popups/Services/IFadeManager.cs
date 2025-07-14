using System;

namespace Game.Popups
{
	public interface IFadeManager
	{
		void FadeIn(Action onComplete = null);
		void FadeOut(Action onComplete = null);
	}
}