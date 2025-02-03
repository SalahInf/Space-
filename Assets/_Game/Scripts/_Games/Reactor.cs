using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Reactor : MonoBehaviour
{
    public bool isFull;
    public bool isFilling;
    public Rocket m_rocket;
    public int currentResourceLevel { get; private set; }
    public int maxResouceCount => m_rocket.reactosResourceCount;
    public Transform target;

    [SerializeField] Image m_image;
    [SerializeField] TMP_Text m_count;
    [SerializeField] float speed;

    [SerializeField] Transform m_engrenage1;
    [SerializeField] Transform m_engrenage2;

    [SerializeField] SkinnedMeshRenderer m_rend;
    [SerializeField] ParticleSystem m_particle;
    private int tmpValue;
    private void Start()
    {
        //m_count.text = $"{tmpValue} / {maxResouceCount}";
        m_count.text = "";
    }
    private void Update()
    {
       // m_engrenage1.Rotate(Vector3.up * speed * Time.deltaTime);
       // m_engrenage2.Rotate(-Vector3.up * speed * Time.deltaTime);
        //m_engrenage2.RotateAround(transform.position,Vector3.right , 20 * Time.deltaTime);
    }

    public void AddResources(int resourceCount, float time)
    {

        if (currentResourceLevel < maxResouceCount)
        {
            StartCoroutine(SmoothChange(resourceCount, time));
        }
    }

    public void UpdateValue()
    {
        tmpValue++;
        //m_count.text = $"{tmpValue} / {maxResouceCount}";
        m_count.text = tmpValue == maxResouceCount ?"Max" :"";
    }
    IEnumerator SmoothChange(int resouces, float time = 0.5f)
    {
        float tmpSpeed = speed;
        float t = 0;
        speed *= 2;
        float targetWeight = (Mathf.InverseLerp(maxResouceCount, 0, resouces + currentResourceLevel)) * 100;
        print(targetWeight);
        float weight = m_rend.GetBlendShapeWeight(0);
        m_particle.transform.localScale = Vector3.zero;
        m_particle.gameObject.SetActive(true);
        m_particle.transform.DOScale(Vector3.one, time);
        currentResourceLevel += resouces;
        isFull = currentResourceLevel >= maxResouceCount ? true : false;
        while (t < time)
        {
            t += Time.deltaTime;
            speed = Mathf.Lerp(speed, speed * 1.1f, t / time);
            float value = Mathf.Lerp(weight, targetWeight, t / time);
            m_rend.SetBlendShapeWeight(0, value);
            m_image.fillAmount = (100 - value) / 100;
            yield return null;
        }
        m_particle.transform.DOScale(Vector3.zero, 0.4f).OnComplete(() => m_particle.gameObject.SetActive(false));
        if (isFull)
            m_rocket.Fetch();
        speed = tmpSpeed;
        isFilling = false;
    }

}
