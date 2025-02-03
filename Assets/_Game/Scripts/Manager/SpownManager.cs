using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpownManager : MonoBehaviour
{
    [SerializeField] PlayerController m_player;
    [SerializeField] Transform m_partent;
    private Level currentLevel;
    private PlayerController currentPlayer;
    public void Init()
    {
        SpownLevel();
    }

    void SpownLevel()
    {

        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
            Destroy(currentPlayer.gameObject);
        }

        currentLevel = Instantiate(Root.levelManager.InitNewLevel(), m_partent);
        currentLevel.transform.position = Vector3.zero;
        Root.CameraManager.m_endLevelCamera = currentLevel.endLevelCamera;
        currentPlayer = Instantiate(m_player, m_partent);
        currentPlayer.transform.position = currentLevel.startPos.position;
        currentPlayer.lines = currentLevel.lines;
        Root.Upgrademanager.playerController = currentPlayer;

        if (Root.GameManager.currentLevel == 0)
            currentLevel.VisibleTutoRocks(Root.GameManager.dataSaved.isFirstTime);
    }


}
