using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController playerController => Root.Upgrademanager.playerController;
    UpgradeManager upgradeManager => Root.Upgrademanager;
    public bool gameStart => _gameStarted;
    public bool isPlaying;

    public int currentLevel;
    private int _currentPlayerGold;
    private int _currentPlayerJems;
    private int _currentPlayerOil;
    private int _currentPlayerElectricity;

    [SerializeField] private int _maxPlayerStorage;
    private int _levelGain;
    private bool _gameStarted = false;
    private int _currentStorage;

    public bool isBaseCharged;

    public int MaxSacStorage
    {
        get => _maxPlayerStorage;
        set
        {
            _maxPlayerStorage = value;
            UpdateUiValues();
        }
    }
    public int CurrentStorage
    {
        get => _currentStorage;
        set
        {
            _currentStorage = value;
            UpdateUiValues();
        }
    }
    public int CurrentPlayerGold
    {
        get => _currentPlayerGold;
        set
        {
            _currentPlayerGold = value;
            UpdateUiValues();
        }
    }
    public int CurrentPlayerJems
    {
        get => _currentPlayerJems;
        set
        {
            _currentPlayerJems = value;
            UpdateUiValues();
            if (_currentPlayerJems < _maxPlayerStorage)
            {
                playerController.HideMaxUi();
            }
        }
    }
    public int CurrentPlayerOil
    {
        get => _currentPlayerOil;
        set
        {
            _currentPlayerOil = value;
            UpdateUiValues();
            if (_currentPlayerJems < _maxPlayerStorage)
            {
                playerController.HideMaxUi();
            }
        }
    }
    public int CurrentPlayerElectricity
    {
        get => _currentPlayerElectricity;
        set
        {
            _currentPlayerElectricity = value;
            UpdateUiValues();
            if (_currentPlayerJems < _maxPlayerStorage)
            {
                playerController.HideMaxUi();
            }
        }
    }

    private int _level = 1;
    public SavedData dataSaved;

    [SerializeField] float fetchDataTimer;
    float t;
    public int PlayerLevel
    {
        get => _level;
        set
        {
            _level = value;
            Root.Upgrademanager.UpdateLevelText();
        }
    }
    void UpdateUiValues()
    {
        ColectiblesUi.instance.UpdateColectiblesText(_currentPlayerGold, _currentPlayerJems, CurrentPlayerOil, _currentPlayerElectricity);
    }
    public void Init()
    {
        //Application.targetFrameRate = 60;
        LoadData();
    }
    private void Update()
    {
        t += Time.deltaTime;
        if (t > fetchDataTimer)
        {
            t = 0;
            SaveData();
        }
    }

    public void Reset()
    {
        _levelGain = 0;
        _gameStarted = false;
        //Root.Level.SetProgression(1);
    }

    public void StartGame()
    {
        //Time.timeScale = 1.25f; 
        _gameStarted = true;
        isPlaying = true;

        if (dataSaved.isFirstTime)
            StartCoroutine(TutorialLevel.instance.Tutorial());

        Root.StartLevel();
    }

    public void InitCurrentLevel()
    {
        currentLevel = dataSaved.GetCurrentLevel();
    }
    public void GetData()
    {
        if (dataSaved.isFirstTime)
            return;

        MaxSacStorage = dataSaved.GetMaxStorage();
        CurrentStorage = dataSaved.GetCurrentStorage();
        CurrentPlayerGold = dataSaved.GetGoldCount();
        CurrentPlayerJems = dataSaved.GetJemsCount();
        CurrentPlayerOil = dataSaved.GetOil();
        CurrentPlayerElectricity = dataSaved.GetElectricity();
        PlayerLevel = dataSaved.GetCurrentPlayerLevel();

        (int, int, int, int) levelUpgrades = dataSaved.GetLevesUpgrades();
        (int, int, int, int) goldUpgrades = dataSaved.GetGoldUpgrades();

        upgradeManager.m_currentJetUpgradeLevel = levelUpgrades.Item1;
        upgradeManager.m_currentSacUpgradeLevel = levelUpgrades.Item2;
        upgradeManager.m_currentDamageUpgradeLevel = levelUpgrades.Item3;
        upgradeManager.m_currentSpeedUpgradeLevel = levelUpgrades.Item4;

        upgradeManager.m_currentJetGold = goldUpgrades.Item1;
        upgradeManager.m_currentSacGold = goldUpgrades.Item2;
        upgradeManager.m_currentAxeDamageGold = goldUpgrades.Item3;
        upgradeManager.m_currentAxeSpeedGold = goldUpgrades.Item4;
    }
    void AssigneData()
    {
        dataSaved.AssigneColectibles(CurrentPlayerGold, CurrentPlayerJems, CurrentPlayerElectricity, CurrentPlayerOil);
        dataSaved.AssigneCurrentStorage(CurrentStorage);
        dataSaved.AssigneCurrentLevel(PlayerLevel);
        dataSaved.AssigneMaxStorage(MaxSacStorage);
        dataSaved.AssigneDataSpeed(playerController.axeSpeed, playerController.flySpeed);
        dataSaved.AssigneLevesUpgrades(upgradeManager.m_currentJetUpgradeLevel, upgradeManager.m_currentSacUpgradeLevel, upgradeManager.m_currentDamageUpgradeLevel, upgradeManager.m_currentSpeedUpgradeLevel);
        dataSaved.AssigneGoldUpgrades(upgradeManager.m_currentJetGold, upgradeManager.m_currentSacGold, upgradeManager.m_currentAxeDamageGold, upgradeManager.m_currentAxeSpeedGold);
        dataSaved.SetCurrentLevel(currentLevel);
    }

    public void GameWin()
    {
        if (_gameStarted)
        {
            _gameStarted = false;
            //Root.Level.SetProgression(1);
            currentLevel++;
            SaveData();
            Debug.Log("Win Game");
            Root.LevelWon(_levelGain);
        }
    }

    public void LoseGame()
    {
        if (_gameStarted)
        {
            _gameStarted = false;
            Root.Level.SetProgression(0);
            Debug.Log("lose Game");
            Root.LevelLost(_levelGain);
        }
    }

    internal void GainCurrency(int score)
    {
        _levelGain += score;
        Root.Currency.AddValue(score);
    }

    public void LoadData()
    {
        dataSaved = new SavedData();
        dataSaved.isFirstTime = true;
        SavedData d = Save.LoadFile<SavedData>();
        if (d != null)
        {
            dataSaved = d;
        }
    }
    public void SaveData()
    {
        if (dataSaved == null)
            return;
        dataSaved.isFirstTime = false;
        AssigneData();
        Save.SaveFile<SavedData>(dataSaved);
    }
    public void EndTaksTutorial(int index)
    {
        if (currentLevel == 0)
        {
            TutorialLevel.instance.TaskFinished(index);
        }
    }
}
