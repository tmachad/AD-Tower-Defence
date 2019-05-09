using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public float m_StartingMoney;
    public float m_Money
    {
        get
        {
            return _m_Money;
        }
        set
        {
            m_MoneyText.text = string.Format("${0}", value);
            _m_Money = value;
        }
    }
    private float _m_Money;
    [SerializeField]
    private Text m_MoneyText;

    public int m_StartingLives;
    public int m_Lives
    {
        get
        {
            return _m_Lives;
        }
        set
        {
            m_LivesText.text = string.Format("{0} lives", value);
            _m_Lives = value;
        }
    }
    private int _m_Lives;
    [SerializeField]
    private Text m_LivesText;

    public int m_UpgradeAwardInterval = 5;

    public GameObject m_HUD;
    public GameObject m_MainMenu;
    public GameObject m_GameOverPanel;
    public GameObject m_UpgradePanel;

    private void Awake()
    {
        Instance = this;

        // Set money and lives to update text
        m_Money = m_StartingMoney;
        m_Lives = m_StartingLives;

        m_HUD.SetActive(false);
        m_MainMenu.SetActive(true);
        m_GameOverPanel.SetActive(false);
        m_UpgradePanel.SetActive(false);
    }

    private void Start()
    {
        WaveManager.Instance.WaveChanged += OnWaveChanged;
    }

    private void OnWaveChanged(object sender, WaveEventArgs e)
    {
        if (e.wave % m_UpgradeAwardInterval == 0)
        {
            m_UpgradePanel.SetActive(true);
            m_UpgradePanel.GetComponent<UpgradePanel>().Init();
            Time.timeScale = 0.0f;
        }
    }

    public void StartLevel(Level level)
    {
        m_HUD.SetActive(true);
        m_MainMenu.SetActive(false);

        LevelManager.Instance.LoadLevel(level);
    }

    public void EnemyReachedEnd()
    {
        m_Lives--;

        if (m_Lives <= 0)
        {
            if (HighscoreManager.Instance.SubmitHighscore(LevelManager.Instance.m_LoadedLevel, WaveManager.Instance.m_WaveNumber))
            {
                HighscoreManager.Instance.Save();
            }

            m_GameOverPanel.SetActive(true);
            m_GameOverPanel.GetComponent<GameOverPanel>().Refresh();
            Time.timeScale = 0f;
        }
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        m_Money = m_StartingMoney;
        m_Lives = m_StartingLives;

        WaveManager.Instance.ResetToInitialState();
        LevelManager.Instance.ResetToInitialState();
        TowerManager.Instance.ResetToInitialState();
        EnemyManager.Instance.ResetToInitialState();

        m_HUD.SetActive(false);
        m_MainMenu.SetActive(true);
        m_GameOverPanel.SetActive(false);
    }
}
