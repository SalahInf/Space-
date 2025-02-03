using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Batery : MonoBehaviour
{
    [SerializeField] Image bateryImg;
    public void Blink()
    {
        bateryImg.gameObject.SetActive(true);
        bateryImg.DOFade(0.3f, 0.65f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo).SetId(this);
    }

    public void StopBlinking()
    {
        DOTween.Kill(this);
        bateryImg.DOFade(0, 0.5f).SetEase(Ease.InElastic).SetId(this).OnComplete(() => bateryImg.gameObject.SetActive(false));
    }

    public void ChangeColor(Color color, float time)
    {
        bateryImg.DOColor(color, time).SetEase(Ease.Flash);
    }
}
