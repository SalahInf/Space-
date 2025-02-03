using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera m_mainPlayerCamera;
    [SerializeField] CinemachineVirtualCamera m_upgradLevelCamera;

    public CinemachineVirtualCamera m_endLevelCamera;

    public CinemachineBasicMultiChannelPerlin m_cameraShake;

    [SerializeField] float m_timeShake;
    [SerializeField] float m_amplitude;
    [SerializeField] float m_frequency;

    public void Init()
    {
        m_mainPlayerCamera.LookAt = Root.Upgrademanager.playerController.transform;
        m_mainPlayerCamera.Follow = Root.Upgrademanager.playerController.transform;

        m_upgradLevelCamera.LookAt = Root.Upgrademanager.playerController.transform;
        m_upgradLevelCamera.Follow = Root.Upgrademanager.playerController.transform;

        m_cameraShake = m_endLevelCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        m_endLevelCamera.gameObject.SetActive(false);
        m_cameraShake.m_AmplitudeGain = 0;
        m_cameraShake.m_AmplitudeGain = 0;
    }

    public void ActivateEndCamera()
    {
        m_endLevelCamera.gameObject.SetActive(true);
        StartCoroutine(Shake(m_timeShake, m_amplitude, m_frequency));
    }

    public void ActivateCamera(bool state)
    {
        m_upgradLevelCamera.gameObject.SetActive(state);
    }

    IEnumerator Shake(float time, float amplitude, float frequency)
    {
        float t = 0;

        yield return new WaitForSeconds(0.8f);
        m_cameraShake.m_AmplitudeGain = 1;
        while (t <= time)
        {
            t += Time.deltaTime;
            m_cameraShake.m_FrequencyGain = Mathf.Lerp(0, frequency, t / time);
            yield return null;
        }
        m_cameraShake.m_AmplitudeGain = 0;
        m_cameraShake.m_FrequencyGain = 0;
    }
}
