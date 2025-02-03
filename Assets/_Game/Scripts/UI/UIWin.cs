using DG.Tweening;
using UnityEngine;

namespace UIParty
{
    public class UIWin : UIView
    {
        [SerializeField] private RectTransform _winRect;
        [SerializeField] private ButtonUI _nextButton;

        public override void Init()
        {
            base.Init();
            _nextButton.Init(NextLevel);

            _winRect.anchoredPosition = new Vector2(_winRect.anchoredPosition.x, 700);
            _nextButton.Rect.localScale = Vector3.zero;
        }

        protected override void ShowView()
        {
            DOTween.Kill(gameObject);
            _winRect.DOAnchorPosY(0, .35f).SetId(gameObject);
            _nextButton.Rect.DOScale(1, .35f).SetEase(Ease.OutBack).SetId(gameObject).OnComplete(FinShow);
        }

        protected override void CloseView()
        {
            DOTween.Kill(gameObject);
            _winRect.DOAnchorPosY(700, .2f).SetId(gameObject);
            _nextButton.Rect.DOScale(0, .2f).SetId(gameObject).OnComplete(FinClose);
        }

        protected override void OnStarClosing()
        {
            Root.EndWindowStartClose();
        }

        protected override void OnFinishClosing()
        {
            Root.LevelWonClosed();
        }

        private void NextLevel()
        {
            Close();
        }
    }
}
