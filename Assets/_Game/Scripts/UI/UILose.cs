using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIParty
{
    public class UILose : UIView
    {
        [SerializeField] private RectTransform _LoseRect;
        [SerializeField] private ButtonUI _retryButton;

        public override void Init()
        {
            base.Init();
            _retryButton.Init(RetryLevel);

            _LoseRect.anchoredPosition = new Vector2(_LoseRect.anchoredPosition.x, 700);
            _retryButton.Rect.localScale = Vector3.zero;
        }

        protected override void ShowView()
        {
            DOTween.Kill(gameObject);
            _LoseRect.DOAnchorPosY(0, .35f).SetId(gameObject);
            _retryButton.Rect.DOScale(1, .35f).SetEase(Ease.OutBack).SetId(gameObject).OnComplete(FinShow);
        }

        protected override void CloseView()
        {
            DOTween.Kill(gameObject);
            _LoseRect.DOAnchorPosY(700, .2f).SetId(gameObject);
            _retryButton.Rect.DOScale(0, .2f).SetId(gameObject).OnComplete(FinClose);
        }

        protected override void OnStarClosing()
        {
            Root.EndWindowStartClose();
        }

        protected override void OnFinishClosing()
        {
            Root.LevelLoseClosed();
        }

        private void RetryLevel()
        {
            Close();
        }
    }
}