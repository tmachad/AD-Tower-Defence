using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour {

    public string m_HighscoreDisplayText;
    public Text m_TitleText;
    public Text m_HighScoreText;
    public Image m_PreviewImage;
    public Button m_StartButton;

    private Level m_Level;

    public void Init(Level level)
    {
        m_Level = level;
        m_TitleText.text = level.name;
        m_HighScoreText.text = string.Format(m_HighscoreDisplayText, HighscoreManager.Instance.GetHighscore(level));
        m_PreviewImage.sprite = level.m_PreviewSprite;
        m_PreviewImage.preserveAspect = true;

        m_StartButton.onClick.AddListener(() =>
        {
            GameManager.Instance.StartLevel(m_Level);
        });
    }
}
