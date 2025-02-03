using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] AllLevels levels;
    public Level InitNewLevel()
    {
        int propgress = Root.GameManager.currentLevel <= levels.lvl.Length ? Root.GameManager.currentLevel
            : Random.Range(0, levels.lvl.Length);

        int levelCtr = (Mathf.Clamp(propgress, 0, levels.lvl.Length - 1));
        //RenderSettings.skybox = levels.material[0];
        return levels.lvl[levelCtr];
    }
}

[System.Serializable]

public struct AllLevels
{
    public Level[] lvl;
    public Material[] material;
}
