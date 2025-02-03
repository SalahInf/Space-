using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ElectricityGenerator : MonoBehaviour
{
    [SerializeField] float timeToWait;
    [SerializeField] Image WaitImage;
    [SerializeField] Color targetColor;
    Color startFxColor;
    ParticleSystem.MainModule settings;
    [SerializeField] ParticleSystem pFx;
    [SerializeField] ParticleSystem openFx;
    [SerializeField] float waitTimeSend;
    [SerializeField] Dimands dime;
    [SerializeField] Transform targetPos;
    [SerializeField] ParticleSystem explodFx;
    [SerializeField] ParticleSystem crushFx;

    [SerializeField] float timeForOneJem;
    [SerializeField] int maxReactorCapacity;
    [SerializeField] int currentCapacity;
    [SerializeField] float currentCapacityValue;


    [SerializeField] Image EnergyBar;
    Coroutine energyCorotine;
    public bool isInZone;
    float t = 0;
    float currentEnergyStatue;
    bool isCharging;

    bool needCharging;
    private void Start()
    {
        settings = pFx.main;
        startFxColor = settings.startColor.color;
        currentCapacity = maxReactorCapacity;
        currentCapacityValue = Mathf.InverseLerp(0, maxReactorCapacity, currentCapacity);
        EnergyBar.fillAmount = currentCapacityValue;
        currentEnergyStatue = 1;
        Root.GameManager.isBaseCharged = true;
    }
    private void Update()
    {
        if (isCharging || needCharging || !Root.GameManager.gameStart)
            return;

        t += Time.deltaTime;
        if (currentEnergyStatue <= 0)
        {
            print("Low Energy !!!!!!!!!!!!");
            Root.GameManager.isBaseCharged = false;
            needCharging = true;
            return;
        }

        if (t >= timeForOneJem)
        {
            t = 0;
            currentCapacity--;

            currentCapacityValue = Mathf.InverseLerp(0, maxReactorCapacity, currentCapacity);
            if (energyCorotine != null)
                StopCoroutine(energyCorotine);

            energyCorotine = StartCoroutine(SmoothChangeValue(timeForOneJem / 1.3f));
        }
    }
    IEnumerator SmoothChangeValue(float time)
    {
        float t1 = 0;
        float amount = EnergyBar.fillAmount;
        if (currentCapacity == 2)
        {
            Root.UIManager.batery.ChangeColor(Color.white, time);
            Root.UIManager.batery.Blink();
        }
        if (currentCapacity == 1)
        {
            Root.UIManager.batery.ChangeColor(Color.red, time);
        }
        while (t1 <= time)
        {
            t1 += Time.deltaTime;
            EnergyBar.fillAmount = Mathf.Lerp(amount, currentCapacityValue, t1 / time);
            currentEnergyStatue = EnergyBar.fillAmount;
            yield return null;
        }
    }
    public IEnumerator OpenGenerator(PlayerController player)
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
            // Send Objects
            StartCoroutine(SendElectricity(player));
        }
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

    IEnumerator SendElectricity(PlayerController player)
    {
        //Vector3 startScale = crushFx.transform.localScale;
        //int count = Root.GameManager.CurrentPlayerElectricity;
        //crushFx.transform.localScale = Vector3.zero;
        //crushFx.gameObject.SetActive(true);
        //crushFx.transform.DOScale(startScale, 0.2f);

        if (Root.GameManager.CurrentPlayerElectricity > 0)
        {
            Root.GameManager.isBaseCharged = true;
            Root.UIManager.batery.StopBlinking();
            isCharging = true;
            needCharging = false;
            crushFx.Play(true);
        }

        while (Root.GameManager.CurrentPlayerElectricity > 0)
        {
            Dimands d = Instantiate(dime, player.m_startLine.position, Quaternion.identity);
            d.transform.DOJump(targetPos.position, 1, 1, 0.5f).OnComplete(() =>
            {
                ParticleSystem p = Instantiate(explodFx, targetPos.position, Quaternion.identity);
                Destroy(p.gameObject, 0.6f);
                Destroy(d.gameObject);
            });

            Root.GameManager.CurrentPlayerElectricity--;
            if (Root.GameManager.CurrentStorage > 0)
                Root.GameManager.CurrentStorage--;
            currentCapacity++;
            if (energyCorotine != null)
                StopCoroutine(energyCorotine);

            energyCorotine = StartCoroutine(SmoothChangeValue(timeForOneJem / 2));
            currentCapacityValue = Mathf.InverseLerp(0, maxReactorCapacity, currentCapacity);

            yield return new WaitForSeconds(waitTimeSend);
        }
        Root.GameManager.EndTaksTutorial(5);
        //crushFx.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => crushFx.gameObject.SetActive(false));
        crushFx.Pause(true);
        isCharging = false;
    }
}
