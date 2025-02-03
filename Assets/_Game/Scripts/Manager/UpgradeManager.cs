using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UIParty;
using TMPro;
using Sirenix.OdinInspector;

public class UpgradeManager : MonoBehaviour
{
    public PlayerController playerController;
    private ColectiblesUi _colectiblesUi => ColectiblesUi.instance;

    #region Level Upgrade    
    [SerializeField] TMP_Text m_levelTxt;
    [SerializeField] GameObject m_levelHolder;
    [SerializeField] CanvasGroup m_levelUpPanel;
    #endregion

    [SerializeField] RectTransform m_rect;
    [SerializeField] GameObject m_Axeholder;
    [SerializeField] GameObject m_Jetholder;
    [SerializeField] GameObject m_Sacholder;

    [SerializeField] ButtonUI btnUpGradeAxeSpeed;
    [SerializeField] ButtonUI btnUpGradeAxeDamage;
    [SerializeField] ButtonUI btnUpGradeJet;
    [SerializeField] ButtonUI btnUpGradeSac;
    [SerializeField] Animator LevelUpAnimator;

    // Jet Value
    #region JetValues
    [ReadOnly] public int m_currentJetUpgradeLevel;
    public int m_currentJetGold;

    [SerializeField] int m_maxLevelsJet;
    #endregion

    #region Sac Value
    public int m_currentSacGold;
    [ReadOnly] public int m_currentSacUpgradeLevel;

    [SerializeField] int m_maxLevelsSac;
    #endregion
    // Axe Upgrades
    #region AxeUpgrade
    [ReadOnly] public int m_currentDamageUpgradeLevel;
    [ReadOnly] public int m_currentSpeedUpgradeLevel;
    public int m_currentAxeSpeedGold;
    public int m_currentAxeDamageGold;

    [SerializeField] int m_maxLevelsAxeDamage;
    [SerializeField] int m_maxLevelsAxeSpeed;

    #endregion

    [SerializeField] int m_upgradePerUnit;
    [SerializeField] float multiplayer;
    void Update()
    {
        //Testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Root.GameManager.CurrentPlayerGold += 100;
            Root.GameManager.CurrentPlayerJems += 100;
            Root.GameManager.CurrentPlayerOil += 100;
            Root.GameManager.CurrentPlayerElectricity += 100;
        }
    }

    public void Init()
    {
        btnUpGradeAxeSpeed.Init(UpgradeAxeSpeed);
        btnUpGradeAxeDamage.Init(UpgradeAxeDamage);
        btnUpGradeJet.Init(UpgradeJet);
        btnUpGradeSac.Init(UpgradeSac);
        _colectiblesUi.UpdateColectiblesText(0, 0, 0, 0);
        InitValues();
        Close();
    }

    #region UpgradeSysteme
    void UpgradeAxeSpeed()
    {
        if (m_currentSpeedUpgradeLevel >= m_maxLevelsAxeSpeed)
            return;

        if (Root.GameManager.CurrentPlayerGold >= m_currentAxeSpeedGold)
        {
            m_currentSpeedUpgradeLevel++;
            playerController.UpgradeAxe(m_maxLevelsAxeSpeed, 0);
            Root.GameManager.CurrentPlayerGold -= m_currentAxeSpeedGold;
            m_currentAxeSpeedGold = IncreseGold(m_currentAxeSpeedGold, m_currentSpeedUpgradeLevel);
            StartCoroutine(UpgradeLevel());
            InitValues();
            Root.GameManager.EndTaksTutorial(3);
        }
        UpgradeStatue();

    }
    void UpgradeAxeDamage()
    {
        if (m_currentDamageUpgradeLevel >= m_maxLevelsAxeDamage)
            return;


        if (Root.GameManager.CurrentPlayerGold >= m_currentAxeDamageGold)
        {
            m_currentDamageUpgradeLevel++;
            playerController.UpgradeAxe(0, 1);
            Root.GameManager.CurrentPlayerGold -= m_currentAxeDamageGold;
            m_currentAxeDamageGold = IncreseGold(m_currentAxeDamageGold, m_currentDamageUpgradeLevel);
            InitValues();
            StartCoroutine(UpgradeLevel());
        }
        UpgradeStatue();
    }

    IEnumerator UpgradeLevel()
    {
        int value = (m_currentDamageUpgradeLevel + m_currentSpeedUpgradeLevel) % 2;
        if (value == 0)
        {
            Root.CameraManager.ActivateCamera(true);
            m_levelUpPanel.gameObject.SetActive(true);
            m_levelUpPanel.DOFade(0.5f, 0.1f);
            Root.GameManager.isPlaying = false;
            LevelUpAnimator.SetTrigger("LevelUp");
            yield return new WaitForSeconds(0.35f);
            Root.GameManager.PlayerLevel++;
            Root.GameManager.isPlaying = true;
            Root.Upgrademanager.playerController.ActivateUpgradeLvlFx(true);
            yield return new WaitForSeconds(0.9f);
            m_levelUpPanel.DOFade(0, 0.05f).OnComplete(() => m_levelUpPanel.gameObject.SetActive(false));
            Root.Upgrademanager.playerController.ActivateUpgradeLvlFx(false);
            Root.CameraManager.ActivateCamera(false);
        }
        UpgradeStatue();
    }

    public void UpdateLevelText() => m_levelTxt.text = Root.GameManager.PlayerLevel.ToString();
    void UpgradeJet()
    {
        if (m_currentJetUpgradeLevel >= m_maxLevelsJet)
            return;

        if (Root.GameManager.CurrentPlayerGold >= m_currentJetGold)
        {
            m_currentJetUpgradeLevel++;
            playerController.UpgradeJet(m_maxLevelsJet);
            Root.GameManager.CurrentPlayerGold -= m_currentJetGold;
            m_currentJetGold = IncreseGold(m_currentJetGold, m_currentJetUpgradeLevel);
            InitValues();
        }
        UpgradeStatue();
    }

    void UpgradeSac()
    {
        if (m_currentSacUpgradeLevel >= m_maxLevelsSac)
            return;

        if (Root.GameManager.CurrentPlayerGold >= m_currentSacGold)
        {
            m_currentSacUpgradeLevel++;
            playerController.UpgradeSac(1);
            Root.GameManager.CurrentPlayerGold -= m_currentSacGold;
            m_currentSacGold = IncreseGold(m_currentSacGold, m_currentSacUpgradeLevel);
            InitValues();
        }
        UpgradeStatue();
    }

    int IncreseGold(int gold, int level)
    {
        int u = m_upgradePerUnit * level;
        gold += u;
        return gold;
    }
    void InitValues()
    {
        btnUpGradeAxeDamage.axeDamagLevelTxt.text = m_currentDamageUpgradeLevel == m_maxLevelsAxeDamage ? "Max" : m_currentDamageUpgradeLevel.ToString();
        btnUpGradeAxeDamage.axeDamagGoldTxt.text = m_currentDamageUpgradeLevel == m_maxLevelsAxeDamage ? "Max" : m_currentAxeDamageGold.ToString();

        btnUpGradeAxeSpeed.axeSpeedLevelTxt.text = m_currentSpeedUpgradeLevel == m_maxLevelsAxeDamage ? "Max" : m_currentSpeedUpgradeLevel.ToString();
        btnUpGradeAxeSpeed.axeSpeedGoldTxt.text = m_currentSpeedUpgradeLevel == m_maxLevelsAxeDamage ? "Max" : m_currentAxeSpeedGold.ToString();

        btnUpGradeJet.jetLevelTxt.text = m_currentJetUpgradeLevel == m_maxLevelsJet ? "Max" : m_currentJetUpgradeLevel.ToString();
        btnUpGradeJet.jetGoldTxt.text = m_currentJetUpgradeLevel == m_maxLevelsJet ? "Max" : m_currentJetGold.ToString();

        btnUpGradeSac.sacLevelTxt.text = m_currentSacUpgradeLevel == m_maxLevelsSac ? "Max" : m_currentSacUpgradeLevel.ToString();
        btnUpGradeSac.sacGoldTxt.text = m_currentSacUpgradeLevel == m_maxLevelsSac ? "Max" : m_currentSacGold.ToString();
    }
    #endregion

    public void Open(TypeUpgrade type)
    {
        DOTween.Kill(this);
        if (type == TypeUpgrade.axeUpgrade)
        {
            UpgradeStatue();
            m_Axeholder.SetActive(true);
        }
        else if (type == TypeUpgrade.jetUpgrade)
        {
            UpgradeStatue();
            m_Jetholder.SetActive(true);
            Root.GameManager.EndTaksTutorial(6);
        }
        else if (type == TypeUpgrade.sacUpgrade)
        {
            UpgradeStatue();
            m_Sacholder.SetActive(true);
        }
        m_rect.DOScale(Vector3.one, 0.2f).SetEase(Ease.Flash).SetId(this);
        Root.GameManager.EndTaksTutorial(7);
    }
    public void Close()
    {
        DOTween.Kill(this);
        m_rect.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Flash).OnComplete(() =>
        {
            m_Axeholder.SetActive(false);
            m_Jetholder.SetActive(false);
            m_Sacholder.SetActive(false);

        }).SetId(this);

    }

    void UpgradeStatue()
    {
        btnUpGradeAxeSpeed._button.enabled = Root.GameManager.CurrentPlayerGold >= m_currentAxeSpeedGold ? btnUpGradeAxeSpeed.ChangeBtnColor(true) : btnUpGradeAxeSpeed.ChangeBtnColor(false);
        btnUpGradeAxeDamage._button.enabled = Root.GameManager.CurrentPlayerGold >= m_currentAxeDamageGold ? btnUpGradeAxeDamage.ChangeBtnColor(true) : btnUpGradeAxeDamage.ChangeBtnColor(false);
        btnUpGradeJet._button.enabled = Root.GameManager.CurrentPlayerGold >= m_currentJetGold ? btnUpGradeJet.ChangeBtnColor(true) : btnUpGradeJet.ChangeBtnColor(false);
        btnUpGradeSac._button.enabled = Root.GameManager.CurrentPlayerGold >= m_currentSacGold ? btnUpGradeSac.ChangeBtnColor(true) : btnUpGradeSac.ChangeBtnColor(false);

    }

}
