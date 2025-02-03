using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIParty
{
    public class UITuto : UIView
    {
        [SerializeField] private ButtonUI _buttonStart;
        public override void Init()
        {
            base.Init();
            _buttonStart.Rect.anchoredPosition = new Vector2(_buttonStart.Rect.anchoredPosition.x, -600);
            _buttonStart.Init(StartGame);
        }

        protected override void ShowView()
        {
            FinShow();
            DOTween.Kill(gameObject);
            _buttonStart.Rect.DOAnchorPosY(0, .85f).SetEase(Ease.OutBack).SetId(gameObject);
        }

        protected override void CloseView()
        {
            DOTween.Kill(gameObject);
            _buttonStart.Rect.DOAnchorPosY(-600, .7f).SetId(gameObject).OnComplete(FinClose);
        }

        void StartGame()
        {
            print("Start Game");
            Root.GameManager.StartGame();
        }
    }
}
