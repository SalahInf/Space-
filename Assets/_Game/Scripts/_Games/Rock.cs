using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
public class Rock : MonoBehaviour
{
    public int levelNeeded;
    public bool isDestroyed;
    public List<Dimands> m_dimands;
    [SerializeField] int m_health;
    [SerializeField] int m_colectiblesPerHit;
    [SerializeField] int m_colectGoldPerHit;

    [SerializeField] GameObject[] m_LODVisuals;

    [SerializeField] TMP_Text m_txtlevel;
    [SerializeField] Transform m_Holder;
    [SerializeField] Image m_img;

    [SerializeField] Material m_oilMaterial;
    [SerializeField] Material m_jemsMaterial;
    [SerializeField] Material m_electricityMaterial;

    [SerializeField] float speed;
    [SerializeField] float amplitude;

    float tmpSpeed;
    float tmpAmplitude;
    Vector3 pos;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 180), 0);
        //dime[index].skin.gameObject.SetActive(true);
        m_txtlevel.text = levelNeeded.ToString();
        pos = skin.localPosition;
        //RotateRock();
        RandomRotation();
        ShowLod();
        ChangeType();

        tmpSpeed = Random.Range(speed / 2, speed);
        tmpAmplitude = Random.Range(amplitude / 2, amplitude);
    }
    void ChangeType()
    {
        switch (rockType)
        {
            case RockType.jem:
                foreach (var item in m_dimands)
                {
                    item.mesh.material = m_jemsMaterial;
                }
                break;
            case RockType.elecricity:
                foreach (var item in m_dimands)
                {
                    item.mesh.material = m_electricityMaterial;
                }
                break;
            case RockType.oil:
                foreach (var item in m_dimands)
                {
                    item.mesh.material = m_oilMaterial;
                }
                break;
        }
    }
    private void Update()
    {
        //MoveRock();
    }
    void ShowLod()
    {
        for (int i = 0; i < m_LODVisuals.Length; i++)
        {
            m_LODVisuals[i].SetActive((m_health - 1) == i);
        }
    }
    public void ShowLevel()
    {
        m_Holder.gameObject.SetActive(true);
        m_img.DOFade(1, 0.3f).OnComplete(() => m_img.transform.DOScale(Vector3.one, 1f).OnComplete(() => m_img.DOFade(0, 0.3f).OnComplete(() => m_Holder.gameObject.SetActive(false))));
    }

    public (int, int) Hited(int damage, PlayerController player)
    {
        m_health -= damage;
        ShowLod();
        transform.DOScale(Vector3.one * 0.85f, 0.15f).SetEase(Ease.InQuad).OnComplete(() => transform.DOScale(Vector3.one, 0.1f)).SetId(this);
        if (m_health <= 0)
        {
            DOTween.Kill(this);
            isDestroyed = true;
            Root.GameManager.EndTaksTutorial(rockType == RockType.jem ? 0 : 4);
            player.Attack(false);
            transform.DOScale(0, 0.5f);
        }
        if (m_dimands.Count > 0)
        {
            Dimands dime = m_dimands[0];
            StartCoroutine(dime.MoveToward(player));
            //player.TextPupUp();
            m_dimands.RemoveAt(0);
        }
        if (Root.GameManager.currentLevel == 0)
        {
            return (TutorialLevel.instance.currentTask == 0 ? 5 : m_colectiblesPerHit, m_colectGoldPerHit);
        }
        else
        {
            return (m_colectiblesPerHit, m_colectGoldPerHit);
        }
    }
    void RotateRock()
    {
        // float duration = Random.Range(2, 5);
        //skin.transform.DOLocalMoveY(0.15f, duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetId(this);
    }

    void MoveRock()
    {
        //pos.y = Mathf.Sin(Time.time * tmpSpeed) * tmpAmplitude;

        //skin.localPosition = pos;
    }

    void RandomRotation()
    {
        foreach (Dimands item in m_dimands)
        {
            item.transform.rotation = Random.rotation;
        }
    }
    public Transform skin;
    public RockType rockType;
}

public enum RockType
{
    jem,
    elecricity,
    oil
}
