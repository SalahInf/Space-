using System;
using System.Collections;
using System.Collections.Generic;
using UIParty;
using UnityEngine;

public class Root : MonoBehaviour
{
    private static Root _instance;
    private static bool _initDone = false;

    public static GameManager GameManager => _instance._gameManager;
    public static UIManager UIManager => _instance._uIManager;
    public static DataManager DataManager => _instance._dataManager;
    public static DataLevelValue Level => DataManager.levelData;
    public static DataValue Currency => DataManager.coinData;
    public static UpgradeManager Upgrademanager => _instance._upgradeManager;
    public static Joystick Joystick => _instance._joystick;
    public static SpownManager SpownManager => _instance._spownManager;
    public static CameraManager CameraManager => _instance._cameraManager;
    public static LevelManager levelManager => _instance._levelManager;

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private UIManager _uIManager;
    [SerializeField] private DataManager _dataManager;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private SpownManager _spownManager;
    [SerializeField] private CameraManager _cameraManager;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //_dataManager.Init();
        _gameManager.Init();
        _gameManager.InitCurrentLevel();
        _spownManager.Init();
        _gameManager.GetData();
        _uIManager.Init();
        _cameraManager.Init();
        _initDone = true;
    }

    private void Reset()
    {
        GameManager.Reset();
    }

    internal static void StartLevel()
    {
        UIManager.GameStarted();
    }

    public static void EndWindowStartClose()
    {
    }

    internal static void LevelWon(int levelGain)
    {
        Debug.Log("Level Win" + levelGain);
        UIManager.OpenWinWindow();
    }

    public static void LevelWonClosed()
    {
        Debug.Log("LevelUp");
        Level.LevelUp();

        UIManager.FullSplashScreenOpeen();
    }

    internal static void LevelLost(int levelGain)
    {
        Debug.Log("Level Lose" + levelGain);
        UIManager.OpenLoseWindow();
    }

    public static void LevelLoseClosed()
    {
        Debug.Log("Level Restry");
        Level.SetProgression(0);
        UIManager.FullSplashScreenOpeen();
    }

    public static void ChangeLevel()
    {
    }

    public static void TransitionScreenFull()
    {
        _instance.Reset();
        SpownManager.Init();
        CameraManager.Init();
    }

    public static void TransitionScreenClosed()
    {
        UIManager.OpenMenu();

    }
}
