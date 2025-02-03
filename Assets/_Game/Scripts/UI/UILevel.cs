using UnityEngine;
using DG.Tweening;

namespace UIParty
{
    public class UILevel : UIView
    {
        [SerializeField] private LevelProgressUI _levelProgressUI;

        DataLevelValue _level;

        public void Init(DataLevelValue level)
        {
            Precondition.CheckNotNull(level);

            base.Init();
            _level = level;
            _levelProgressUI.Init(_level);

            _levelProgressUI.Rect.anchoredPosition = new Vector2(_levelProgressUI.Rect.anchoredPosition.x, 350);
        }

        protected override void ShowView()
        {
            DOTween.Kill(gameObject);
            _levelProgressUI.Rect.DOAnchorPosY(0, .35f).SetId(gameObject).OnComplete(FinShow);
        }

        protected override void CloseView()
        {
            DOTween.Kill(gameObject);
            _levelProgressUI.Rect.DOAnchorPosY(300, .2f).SetId(gameObject).OnComplete(FinClose);
        }
    }
}
