using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UIParty
{
    public abstract class UIView : MonoBehaviour
    {
        private Action<UIView> m_onFinishClosing;
        private Action<UIView> m_onFinishShowing;
        [SerializeField, Required, BoxGroup("Window")] protected CanvasGroup _canvasGroup = null;

        private bool _interactable = false;

        public WindowStatus VisibilityStatus { get; private set; }

        public virtual void Init()
        {
            _interactable = _canvasGroup.interactable;
            _canvasGroup.interactable = false;
            gameObject.SetActive(false);
            VisibilityStatus = WindowStatus.Closed;
        }


        public void Open()
        {
            if (VisibilityStatus == WindowStatus.Closed)
            {
                VisibilityStatus = WindowStatus.Opening;
                gameObject.SetActive(true);
                OnStartShowing();
                ShowView();
            }
        }

        public void Close()
        {
            if (VisibilityStatus == WindowStatus.Opening || VisibilityStatus == WindowStatus.Opened)
            {
                VisibilityStatus = WindowStatus.Closing;
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
                OnStarClosing();
                CloseView();
            }
        }


        protected virtual void ShowView()
        {
            FinShow();
        }

        protected virtual void CloseView()
        {
            FinClose();
        }

        protected void FinShow()
        {
            _canvasGroup.interactable = _interactable;

            OnFinishShowing();
            m_onFinishShowing?.Invoke(this);

            VisibilityStatus = WindowStatus.Opened;
        }

        protected void FinClose()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = true;
            gameObject.SetActive(false);
            OnFinishClosing();
            m_onFinishClosing?.Invoke(this);

            VisibilityStatus = WindowStatus.Closed;
        }

        public void RegisterOnFinishClosing(Action<UIView> onFinishClosing)
        {
            m_onFinishClosing += onFinishClosing;
        }

        public void RegisterOnFinishShowing(Action<UIView> onFinishShowing)
        {
            m_onFinishShowing += onFinishShowing;
        }

        public void ClearViewListeners()
        {
            m_onFinishClosing = null;
            m_onFinishShowing = null;
        }

        protected virtual void OnStartShowing() { }

        protected virtual void OnFinishShowing() { }

        protected virtual void OnStarClosing() { }

        protected virtual void OnFinishClosing() { }

    }

    public enum WindowStatus { Opening, Opened, Closing, Closed };

}

