using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TypeUpgrade
{
    jetUpgrade,
    axeUpgrade,
    sacUpgrade
}

public class UpgradeArea : MonoBehaviour
{
    public TypeUpgrade typeOfUpgrade;

    [SerializeField] float timeToWait;
    [SerializeField] Image WaitImage;
    [SerializeField] TMP_Text needImageCharging;
    [SerializeField] Color targetColor;
    Color startFxColor;
    ParticleSystem.MainModule settings;
    [SerializeField] ParticleSystem pFx;
    [SerializeField] ParticleSystem openFx;
    public bool isInZone;
    private void Start()
    {
        settings = pFx.main;
        startFxColor = settings.startColor.color;
        needImageCharging.DOFade(0, 0f).SetEase(Ease.Flash);
    }

    public IEnumerator OpenUpGrade()
    {
        if (Root.GameManager.isBaseCharged)
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
                Root.Upgrademanager.Open(typeOfUpgrade);

            while (true)
            {
                if (!isInZone)
                {
                    settings.startColor = startFxColor;
                    WaitImage.gameObject.SetActive(false);
                    openFx.gameObject.SetActive(false);
                    break;
                }
                yield return null;
            }
        }
        else
        {
            needImageCharging.DOFade(1, 1f).SetEase(Ease.Flash).OnComplete(() => needImageCharging.DOFade(0, 1f).SetEase(Ease.Flash));
        }
    }
}





