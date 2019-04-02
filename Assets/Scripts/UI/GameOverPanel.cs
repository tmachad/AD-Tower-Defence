using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour {

    public string m_WaveReachedMessage;
    public Text m_WaveReachedText;

    public void Refresh()
    {
        m_WaveReachedText.text = string.Format(m_WaveReachedMessage, WaveManager.Instance.m_WaveNumber);
    }
}
