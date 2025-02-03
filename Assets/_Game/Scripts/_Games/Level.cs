using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Level : MonoBehaviour
{
    public Transform startPos;
    public Lines lines;
    public CinemachineVirtualCamera endLevelCamera;
    public GameObject[] tutoRocks;

    public void VisibleTutoRocks(bool isVisible)
    {
        foreach (GameObject item in tutoRocks)
        {
            item.SetActive(isVisible);
        }
    }
}
