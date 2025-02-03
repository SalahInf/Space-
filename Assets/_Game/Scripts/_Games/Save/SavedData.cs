using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedData
{
    private int m_jemsCount;
    private int m_goldCount;
    private int m_electricityCount;
    private int m_oiCount;

    private float m_hitSpeed;
    private float m_flySpeed;

    private int m_currentStorage;
    private int m_maxStorage;
    //private int 

    private int m_currentPlayerLevel;
    public bool isFirstTime;

    public int currentLevel;

    public void AssignePos(Vector3 pos)
    {
        this.pos.posX = pos.x;
        this.pos.posY = pos.y;
        this.pos.posZ = pos.z;
    }

    public void AssigneColectibles(int gold, int jems, int electricity, int oil)
    {
        m_goldCount = gold;
        m_jemsCount = jems;
        m_electricityCount = electricity;
        m_oiCount = oil;
    }

    public void AssigneDataSpeed(float speedhit, float flySpeed)
    {
        m_hitSpeed = speedhit;
        this.m_flySpeed = flySpeed;
    }

    public void AssigneCurrentLevel(int lvl)
    {
        m_currentPlayerLevel = lvl;
    }

    public void AssigneCurrentStorage(int storage)
    {
        m_currentStorage = storage;
    }
    public void AssigneMaxStorage(int maxstorage)
    {
        m_maxStorage = maxstorage;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    public void SetCurrentLevel(int lvl)
    {
        currentLevel = lvl;
    }
    public int GetElectricity()
    {
        return m_electricityCount;
    }
    public int GetOil()
    {
        return m_oiCount;
    }
    public int GetCurrentStorage()
    {
        return m_currentStorage;
    }
    public int GetMaxStorage()
    {
        return m_maxStorage;
    }

    public int GetCurrentPlayerLevel()
    {
        return m_currentPlayerLevel;
    }

    public Vector3 GetPos()
    {
        return new Vector3(pos.posX, pos.posY, pos.posZ);
    }

    public int GetJemsCount()
    {
        return m_jemsCount;
    }

    public (float, float) GetFlySpeed()
    {
        return (m_hitSpeed, m_flySpeed);
    }
    public int GetGoldCount()
    {
        return m_goldCount;
    }

    public void AssigneLevesUpgrades(int jetLevel, int sacLevel, int damageLevel, int speedLevel)
    {
        upgrades.currentJetUpgradeLevel = jetLevel;
        upgrades.currentSacUpgradeLevel = sacLevel;
        upgrades.currentDamageUpgradeLevel = damageLevel;
        upgrades.currentSpeedUpgradeLevel = speedLevel;
    }

    public void AssigneGoldUpgrades(int jetGold, int sacGold, int damageGold, int speedGold)
    {
        upgrades.currentJetGold = jetGold;
        upgrades.currentSacGold = sacGold;
        upgrades.currentAxeDamageGold = damageGold;
        upgrades.currentAxeSpeedGold = speedGold;
    }

    public (int, int, int, int) GetLevesUpgrades()
    {
        return (upgrades.currentJetUpgradeLevel, upgrades.currentSacUpgradeLevel, upgrades.currentDamageUpgradeLevel, upgrades.currentSpeedUpgradeLevel);
    }
    public (int, int, int, int) GetGoldUpgrades()
    {
        return (upgrades.currentJetGold, upgrades.currentSacGold, upgrades.currentAxeDamageGold, upgrades.currentAxeSpeedGold);
    }


    public PlayerPosition pos;
    public Upgrades upgrades;
}

[System.Serializable]
public struct Upgrades
{
    public int currentJetUpgradeLevel;
    public int currentSacUpgradeLevel;

    public int currentDamageUpgradeLevel;
    public int currentSpeedUpgradeLevel;

    public int currentJetGold;
    public int currentSacGold;
    public int currentAxeSpeedGold;
    public int currentAxeDamageGold;
}

[System.Serializable]
public struct PlayerPosition
{
    public float posX;
    public float posY;
    public float posZ;
}

