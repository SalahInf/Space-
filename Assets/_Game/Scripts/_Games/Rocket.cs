using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using TMPro;

public class Rocket : MonoBehaviour
{
    public int reactosResourceCount;
    [SerializeField] int m_reactorCount;
    [SerializeField, ReadOnly] int m_reactorFilled;
    [SerializeField] Transform m_rocketVisual;
    [SerializeField] float m_time;
    [SerializeField] float m_lunchTime;
    [SerializeField] TMP_Text m_rechargingText;

    [SerializeField] GameObject[] m_particles;
    [SerializeField] GameObject m_particleHolder;
    [SerializeField] GameObject[] m_pipes;
    [SerializeField] float m_pipeExplosionForce;
    [SerializeField] GameObject m_pipeExplosionFX;
    [SerializeField] Transform m_playerPosInRocket;
    PlayerController m_playerController => Root.Upgrademanager.playerController;
    public void Fetch()
    {
        m_reactorFilled++;
        if (m_reactorFilled == 4)
        {
            StartCoroutine(GoToSpace());
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    StartCoroutine(GoToSpace());
    }
    void GetIn()
    {
        m_playerController._rb.isKinematic = true;
        m_playerController.transform.GetChild(0).DORotate(new Vector3(-180, 0, 0), 1.5f);
        m_playerController.transform.DOJump(m_playerPosInRocket.position, 2f, 1, 1.5f).OnComplete(() => m_playerController.gameObject.SetActive(false));
        Root.CameraManager.ActivateEndCamera();
        m_playerController.GoInRocket();
    }

    IEnumerator GoToSpace()
    {
        m_particleHolder.SetActive(true);
        Root.Joystick.Init(false);
        GetIn();
        foreach (GameObject particle in m_particles)
        {
            Vector3 startScale = particle.transform.localScale;
            particle.transform.localScale = Vector3.zero;
            particle.transform.DOScale(startScale, 0.3f).SetEase(Ease.InQuad);
        }
        foreach (GameObject pipe in m_pipes)
        {
            pipe.transform.DOShakePosition(m_lunchTime * 1.1f, 0.05f, 10).SetEase(Ease.InQuad).OnComplete(() =>
            {
                Rigidbody rb = pipe.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                Vector3 dir = (Vector3.up + (Vector3.right * Random.Range(0, 1)));
                rb.AddForce(dir * m_pipeExplosionForce, ForceMode.Impulse);
                m_pipeExplosionFX.SetActive(true);
            });
        }
        m_rocketVisual.DOShakePosition(m_lunchTime * 2f, 0.05f, 10).SetEase(Ease.InQuad);
        yield return new WaitForSeconds(m_lunchTime);
        Vector3 target = transform.position + Vector3.up * 20;
        float t = 0;
        Root.GameManager.GameWin();
        while (t <= m_time)
        {
            t += Time.deltaTime;
            m_rocketVisual.position = Vector3.Lerp(m_rocketVisual.position, target, t / m_time);
            yield return null;
        }
    }

    public void ShowRecharging()
    {
        DOTween.Kill(this);
        m_rechargingText.DOFade(1, 1f).SetEase(Ease.Flash).OnComplete(() => m_rechargingText.DOFade(1, 1f).SetEase(Ease.Flash)).SetId(this);
    }

}
