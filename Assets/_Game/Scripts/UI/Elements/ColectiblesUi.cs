using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ColectiblesUi : MonoBehaviour
{
    public static ColectiblesUi instance;

    public TMP_Text sacCount;
    public TMP_Text gold;
    [SerializeField] CanvasGroup m_jemscanvasGroup;
    public TMP_Text jems;
    [SerializeField] CanvasGroup m_oilcanvasGroup;
    public TMP_Text oil;
    [SerializeField] CanvasGroup m_ElectricitycanvasGroup;
    public TMP_Text Electricity;
    [SerializeField] GameObject m_holder;
    [SerializeField] RectTransform m_rect;
    [SerializeField] TMP_Text FPS;
    [SerializeField] float updateTime;

    private void Awake()
    {
        instance = this;
        StartCoroutine(GetFps());
    }


    public void Init()
    {
        Close();

    }

    public void UpdateColectiblesText(int gold, int jems, int oil, int Electricity)
    {

        this.gold.text = gold.ToString();
        m_jemscanvasGroup.DOFade(jems == 0 ? 0 : 1, 0.3f).SetEase(Ease.Flash);
        this.jems.text = jems.ToString();
        m_oilcanvasGroup.DOFade(oil == 0 ? 0 : 1, 0.3f).SetEase(Ease.Flash);
        this.oil.text = oil.ToString();
        m_ElectricitycanvasGroup.DOFade(Electricity == 0 ? 0 : 1, 0.3f).SetEase(Ease.Flash);
        this.Electricity.text = Electricity.ToString();
        sacCount.text = $"{Root.GameManager.CurrentStorage} / {Root.GameManager.MaxSacStorage}";
    }

    public void Open()
    {

        DOTween.Kill(this);

        m_holder.SetActive(true);
        m_rect.DOScale(Vector3.one, 0.3f).SetEase(Ease.InCubic).SetId(this);
    }
    public void Close()
    {
        DOTween.Kill(this);
        m_rect.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InCubic).OnComplete(() => m_holder.SetActive(false)).SetId(this);
    }
    IEnumerator GetFps()
    {
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            if (time >= updateTime)
            {
                FPS.text = ((int)(1f / Time.unscaledDeltaTime)).ToString();
                time = 0;
            }
            yield return null;
        }
    }
}

