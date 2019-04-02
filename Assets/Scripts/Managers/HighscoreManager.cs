using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HighscoreManager : MonoBehaviour {

    public static HighscoreManager Instance;

    private Dictionary<string, int> m_Highscores;
    [SerializeField]
    private string m_FileName;

    private void Awake()
    {
        Instance = this;
        Load();
    }

    public bool SubmitHighscore(Level level, int score)
    {
        if (!m_Highscores.ContainsKey(level.name))
        {
            m_Highscores.Add(level.name, score);
            return true;
        }
        else
        {
            if (score > m_Highscores[level.name])
            {
                m_Highscores[level.name] = score;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public int GetHighscore(Level level)
    {
        if (m_Highscores.ContainsKey(level.name))
        {
            return m_Highscores[level.name];
        }
        else
        {
            return -1;
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + m_FileName);
        bf.Serialize(file, m_Highscores);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/" + m_FileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + m_FileName, FileMode.Open);
            m_Highscores = (Dictionary<string, int>)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            m_Highscores = new Dictionary<string, int>();

            foreach(Level level in LevelManager.Instance.m_Levels)
            {
                m_Highscores.Add(level.name, 0);
            }
        }
    }
}
