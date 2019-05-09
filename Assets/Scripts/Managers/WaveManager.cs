using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : ScalingFactor {

    public event EventHandler<WaveEventArgs> WaveChanged;

    public int m_WaveNumber = 0;
    public Text m_WaveText;

    public ScalingStat m_WaveSize;

    public float m_WaveBufferTime = 15f;
    public Text m_WaveTimerText;

    public GameObject m_NextWaveButton;

    [HideInInspector]
    public SpawnerTile m_Spawner;

    private IEnumerator m_WaveTimerCoroutine;
    private IEnumerator m_SpawnerCoroutine;

    public static WaveManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                Debug.Log("Instance is null, finding object in scene");
                _Instance = FindObjectOfType<WaveManager>();

                if (_Instance == null)
                {
                    Debug.Log("Instance is still null");
                }
            }

            return _Instance;
        }
    }
    private static WaveManager _Instance;

    [SerializeField]
    private Wave[] m_Waves;
    private System.Random rand;

    private void Awake()
    {
        _Instance = this;
        m_WaveText.text = "Wave: " + m_WaveNumber;

        // Initialize wave scaling factors, since they can't be set up ahead of time
        foreach (Wave wave in m_Waves)
        {
            foreach (Wave.Phase phase in wave.m_Phases)
            {
                phase.m_SpawnRate.m_ScalingFactor = this;
            }
        }

        rand = new System.Random();
    }

    public void StartNextWave()
    {
        if (m_WaveTimerCoroutine != null)
        {
            StopCoroutine(m_WaveTimerCoroutine);
        }

        m_WaveNumber++;
        m_WaveText.text = string.Format("Wave: {0}", m_WaveNumber);

        WaveEventArgs eventArgs = new WaveEventArgs()
        {
            wave = m_WaveNumber
        };
        OnWaveChanged(eventArgs);

        Wave nextWave = m_Waves[rand.Next(m_Waves.Length)];

        m_WaveTimerCoroutine = WaveTimer(nextWave);
        StartCoroutine(m_WaveTimerCoroutine);
    }

    public void ResetToInitialState()
    {
        m_WaveNumber = 1;
        m_WaveText.text = "Wave: " + m_WaveNumber;
        m_Spawner = null;
        m_WaveTimerText.text = "Click Next Wave to Start";
        StopCoroutine(m_WaveTimerCoroutine);
        StopCoroutine(m_SpawnerCoroutine);
    }

    public override float GetScalingFactor()
    {
        return m_WaveNumber;
    }

    private IEnumerator WaveTimer(Wave nextWave)
    {
        m_NextWaveButton.SetActive(false);

        m_SpawnerCoroutine = m_Spawner.Spawn(nextWave, m_WaveSize);
        StartCoroutine(m_SpawnerCoroutine);
        
        float duration = nextWave.Duration((int)m_WaveSize) + m_WaveBufferTime;

        while (duration >= 0)
        {
            duration -= Time.deltaTime;
            TimeSpan ts = TimeSpan.FromSeconds(duration);
            m_WaveTimerText.text = string.Format("Next Wave in {0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
            yield return null;
        }

        StartNextWave();
    }

    protected virtual void OnWaveChanged(WaveEventArgs e)
    {
        if (WaveChanged != null)
        {
            WaveChanged.Invoke(this, e);
        }
    }
}

public class WaveEventArgs : EventArgs
{
    public int wave;
}
