using System;
using System.Collections;
using UnityEngine;
using Zenject.Helpers;

namespace Game.Popups
{
	public abstract class BasePopupController<T> : BaseController, IPopupController 
		where T : BasePopupView
	{
		protected T _view;
		private bool _isHiding;
		private CoroutineInfo _animationCoroutine;

		public event Action<PopupType> OnShow;
		public event Action<PopupType> OnShown;
		public event Action<PopupType> OnHide;
		public event Action<PopupType> OnHidden;
		
		public bool IsPopupActive { get; private set; }
		public bool IsReplaced { get; set; }
		public bool ShouldStopCoroutinesOnHide { get; protected set; } = true;
		public PopupType Type { get; private set; }
		public Transform ViewTransform => _view.transform;
		public BasePopupView View => _view;
		public object Data { get; protected set; }
		
		protected override void OnDisposed()
		{
			OnHidden = null;
		}

		public void Init(BasePopupView view, PopupType type)
		{
			Type = type;
			_view = (T) view;
			view.gameObject.SetActive(false);
			_view.CloseClicked += OnCloseClicked;
			OnViewCreated();
		}

		/// <summary>
		/// Called after the view is created and before the popup is shown.
		/// <remarks>Base implementation does nothing.</remarks>
		/// </summary>
		protected virtual void OnViewCreated()
		{
		}

		protected virtual void OnCloseClicked()
		{
			HidePopup();
		}
		
		public void HidePopupImmediate(bool isSilent = false)
		{
			OnHidePopup();
			
			if (!isSilent)
			{
				OnHide?.Invoke(Type);
			}

			_view.HideInstantly();
			HideInternal();
			
			if (!isSilent)
			{
				OnHidden?.Invoke(Type);
			}
		}
		
		public void HidePopup()
		{
			if (_isHiding)
			{
				if (!IsPopupActive && IsReplaced)
				{
					IsReplaced = false;
					OnPopupHidden();
					OnHidden?.Invoke(Type);
				}
				return;
			}
			
			_isHiding = true;
			StopCoroutine(_animationCoroutine);
			_animationCoroutine = StartCoroutine(HidePopupCoroutine(_view.HideCoroutine()));
		}

		private IEnumerator HidePopupCoroutine(IEnumerator animationCoroutine)
		{
			OnHidePopup();
			OnHide?.Invoke(Type);
			yield return new WaitUntil(CanHide);
			yield return animationCoroutine;
			HideInternal();
			OnHidden?.Invoke(Type);
		}
		
		protected virtual bool CanHide()
		{
			return true;
		}
		
		protected virtual void OnHidePopup()
		{
			_view.SetIsInteractable(false);
		}

		private void HideInternal()
		{
			if (ShouldStopCoroutinesOnHide)
			{
				StopAllCoroutines();
			}
			
			OnPopupHidden();
			_view.StopAllCoroutines();
			IsPopupActive = false;
		}
		
		public virtual bool TrySetData(object data = null)
		{
			return true;
		}

		public virtual void ReassignData()
		{
		}

		protected virtual void OnPopupHidden()
		{
		}

		public void ShowPopup()
		{
			_isHiding = false;
			StopCoroutine(_animationCoroutine);
			_animationCoroutine = StartCoroutine(ShowCoroutine());
		}

		private IEnumerator ShowCoroutine()
		{
			IsPopupActive = true;

			_view.SetIsInteractable(true);
			
			OnPopupToShow();
			OnShow?.Invoke(Type);
			
			if (!CanShow())
			{
				yield return new WaitUntil(CanShow);
			}

			_view.gameObject.SetActive(true);
			
			OnPopupActivated();
			
			yield return _view.ShowCoroutine();
			
			OnPopupShown();
			OnShown?.Invoke(Type);
		}
		
		protected virtual bool CanShow()
		{
			return true;
		}
		
		/// <summary>
		/// Called before the popup is shown and before the assets are loaded.
		/// <remarks>Base implementation does nothing.</remarks>
		/// </summary>
		protected virtual void OnPopupToShow(){}

		/// <summary>
		/// Called before the popup is shown and after the assets are loaded.
		/// <remarks>Base implementation does nothing.</remarks>
		/// </summary>
		protected virtual void OnPopupActivated(){}
		
		/// <summary>
		/// Called after the show animation is completed.
		/// <remarks>Base implementation does nothing.</remarks>
		/// </summary>
		protected virtual void OnPopupShown(){}
	}
	
	public abstract class BasePopupController<TView, TData> : BasePopupController<TView> where TView : BasePopupView
	{
		
		public override bool TrySetData(object data = null)
		{
			switch (data)
			{
				case TData popupData:
					Data = popupData;
					SetData(popupData);
					return true;
				case null:
					Data = default;
					SetData(default);
					return true;
				default:
					Debug.LogError($"Popup {Type} requires argument of type {typeof(TData).Name}.");
					return false;
			}
		}
		
		public override void ReassignData()
		{
			SetData((TData)Data);
		}
		
		protected abstract void SetData(TData popupData);
	}
}