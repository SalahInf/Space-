using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

public class TutorialLevel : MonoBehaviour
{
    public static TutorialLevel instance;
    PlayerController player => Root.Upgrademanager.playerController;
    [SerializeField] Transform m_arrowDirection;
    [SerializeField, ReadOnly] Transform m_currentTarget;
    [ReadOnly] public int currentTask;

    bool isDoingTasks;
    public Tasks[] task;
    bool misionStarted = false;
    private void Awake() => instance = this;

    private void Start()
    {
        isDoingTasks = true;
    }

    public IEnumerator Tutorial()
    {
        if (isDoingTasks)
        {
            misionStarted = false;
            for (int i = 0; i < task.Length; i++)
            {
                yield return new WaitForSeconds(0.3f);

                while (!task[i].isFinish)
                {
                    if (!misionStarted)
                    {
                        m_arrowDirection.gameObject.SetActive(false);
                        currentTask = i;
                        task[i].targetCam.gameObject.SetActive(true);
                        task[i].pointer.SetActive(true);
                        OtherPointerVisibility(true);
                        m_currentTarget = task[i].target;
                        misionStarted = true;
                        Root.Joystick.Init(false);
                        m_arrowDirection.transform.position = player.transform.position;
                        m_arrowDirection.transform.forward = m_currentTarget.position - player.transform.position;
                        yield return new WaitForSeconds(task[i].timeToStopCamera);
                        m_arrowDirection.gameObject.SetActive(true);
                        task[i].targetCam.gameObject.SetActive(false);
                        Root.Joystick.Init(true);
                    }
                    m_arrowDirection.transform.position = player.transform.position;
                    m_arrowDirection.transform.forward = m_currentTarget.position - player.transform.position;
                    yield return null;
                }
            }
            yield return null;
        }
    }

    public void TaskFinished(int index)
    {
        if (index < 0)
            return;

        if (isDoingTasks && task[currentTask].taskIndex == index)
        {
            task[currentTask].isFinish = true;
            task[currentTask].pointer.SetActive(false);
            m_arrowDirection.gameObject.SetActive(false);
            OtherPointerVisibility(false);
            misionStarted = false;
        }
    }

    public void OtherPointerVisibility(bool isVisible)
    {
        foreach (GameObject item in task[currentTask].otherPointes)
        {
            item.SetActive(isVisible);
        }
    }
}

[System.Serializable]
public struct Tasks
{
    public CinemachineVirtualCamera targetCam;
    public Transform target;
    public GameObject pointer;
    public GameObject[] otherPointes;
    public int taskIndex;
    public float timeToStopCamera;
    [ReadOnly] public bool isFinish;
}
