using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class SellPoint : MonoBehaviour
{
    [SerializeField] float time;
    public Transform m_startPos;

    public StuckMony stackMony;
    public bool isInZone;
    [SerializeField] float timeToWait;
    [SerializeField] int monyPerEvryJems;
    [SerializeField] ParticleSystem pFx;
    [SerializeField] ParticleSystem openFx;
    [SerializeField] Image WaitImage;
    [SerializeField] Color targetColor;
    [SerializeField] TMP_Text m_rechargingText;
    Color startFxColor;
    ParticleSystem.MainModule settings;
    private void Start()
    {
        settings = pFx.main;
        startFxColor = settings.startColor.color;

    }
    public IEnumerator Sell(int jemsCount, PlayerController player)
    {
        WaitImage.gameObject.SetActive(true);
        openFx.gameObject.SetActive(true);
        float t = 0;
        settings.startColor = targetColor;
        while (t < timeToWait)
        {
            if (!isInZone)
                break;
            t += Time.deltaTime;
            WaitImage.fillAmount = Mathf.Lerp(0, 1, t / timeToWait);
            yield return null;
        }
        if (isInZone)
        {
            int count = 0;
            stackMony.SMony(jemsCount * monyPerEvryJems);
            Root.GameManager.EndTaksTutorial(1);
            while (count < jemsCount)
            {
                count++;
                player.SendJemsToTarget(m_startPos);
                Root.GameManager.CurrentPlayerJems--;
                if (Root.GameManager.CurrentStorage > 0)
                    Root.GameManager.CurrentStorage--;
                yield return new WaitForSeconds(0.05f);
            }
        }
        settings.startColor = startFxColor;
        WaitImage.gameObject.SetActive(false);
        openFx.gameObject.SetActive(false);
    }

    public void ShowRecharging()
    {
        DOTween.Kill(this);
        m_rechargingText.DOFade(1, 1f).SetEase(Ease.Flash).OnComplete(() => m_rechargingText.DOFade(0, 1f).SetEase(Ease.Flash)).SetId(this);
    }
}
