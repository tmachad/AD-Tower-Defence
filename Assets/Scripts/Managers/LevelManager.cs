using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance;

    public Transform m_MapRoot;
    public CameraControls m_Camera;
    public Vector2 m_CameraBoundsPadding;

    public List<Level> m_Levels;

    public RectTransform m_LevelListParent;
    public GameObject m_LevelPanelPrefab;

    public Level m_LoadedLevel { get; private set; }

    private void Awake()
    {
        Instance = this;
        m_LoadedLevel = null;
    }

    private void Start()
    {
        float bufferWidth = m_LevelListParent.rect.width / 2;

        for (int i = 0; i < m_Levels.Count; i++)
        {
            GameObject newPanel = Instantiate(m_LevelPanelPrefab);
            RectTransform trans = newPanel.GetComponent<RectTransform>();
            trans.anchorMin = new Vector2(0f, 0.5f);
            trans.anchorMax = new Vector2(0f, 0.5f);
            trans.pivot = new Vector2(0.5f, 0.5f);
            trans.anchoredPosition = new Vector2(bufferWidth + (trans.rect.width + 25) * i, 0);
            trans.SetParent(m_LevelListParent, false);

            if (i > 0)
            {
                // Only add extra room for panels after the first one
                m_LevelListParent.sizeDelta += new Vector2(trans.rect.width + 25, 0);
            }

            newPanel.GetComponent<LevelPanel>().Init(m_Levels[i]);
        }
    }

    public void LoadLevel(Level level)
    {
        if (level == null)
        {
            throw new ArgumentNullException();
        }

        DestroyLoadedLevel();

        m_LoadedLevel = level;

        Vector2 topLeft = new Vector2(m_MapRoot.transform.position.x - level.m_Width / 2, m_MapRoot.transform.position.y + level.m_Height / 2);

        List<Vector2> worldWaypoints = new List<Vector2>();
        foreach (Vector2Int point in level.m_Waypoints)
        {
            worldWaypoints.Add(topLeft + new Vector2(point.x, -point.y));
        }


        for (int row = 0; row < level.m_Height; row++)
        {
            for (int col = 0; col < level.m_Width; col++)
            {
                Vector3 tilePos = new Vector3(topLeft.x + col, topLeft.y - row);

                switch (level.m_Map[row, col])
                {
                    case Level.Tile.Buildable:
                        Instantiate(level.m_BuildableTile, tilePos, Quaternion.identity, m_MapRoot);
                        break;
                    case Level.Tile.Path:
                        Instantiate(level.m_PathTile, tilePos, Quaternion.identity, m_MapRoot);
                        break;
                    case Level.Tile.Spawner:
                        GameObject tile = Instantiate(level.m_SpawnerTile, tilePos, Quaternion.identity, m_MapRoot);
                        SpawnerTile spawner = tile.GetComponent<SpawnerTile>();
                        spawner.m_Waypoints = worldWaypoints.ToArray();
                        break;
                    case Level.Tile.Goal:
                        Instantiate(level.m_GoalTile, tilePos, Quaternion.identity, m_MapRoot);
                        break;
                }
            }
        }

        m_Camera.m_CameraBounds.extents = new Vector3(level.m_Width / 2 + m_CameraBoundsPadding.x, level.m_Height / 2 + m_CameraBoundsPadding.y);
    }

    public void DestroyLoadedLevel()
    {
        m_LoadedLevel = null;

        foreach(Transform child in m_MapRoot.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ResetToInitialState()
    {
        DestroyLoadedLevel();
    }
}
