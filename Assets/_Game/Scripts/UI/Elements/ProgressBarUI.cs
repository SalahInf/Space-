using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


namespace UIParty
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private Image _transparentImageFill;
        
        public RectTransform Rect => _rect;

        public void SetProgessionRaw (float progression)
        {
            Precondition.CheckNotNull(progression);
            
            _transparentImageFill.DOKill();
            _transparentImageFill.fillAmount = progression;
        }

        public void SetProgression (float progression)
        {
            Precondition.CheckNotNull(progression);

            if (progression == 0)
            {
                SetProgessionRaw(progression);
                return;
            }

            _transparentImageFill.DOComplete();
            _transparentImageFill.DOFillAmount(progression, .1f).SetEase(Ease.Linear);
        }
    }
}