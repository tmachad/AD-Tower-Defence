using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject, ISerializationCallbackReceiver {

    public enum Tile
    {
        Path,
        Buildable,
        Spawner,
        Goal,
        Empty
    };

    public Sprite m_PreviewSprite;

    public GameObject m_PathTile;
    public GameObject m_BuildableTile;
    public GameObject m_SpawnerTile;
    public GameObject m_GoalTile;

    public int m_Width = 10;
    public int m_Height = 10;

    public List<Vector2Int> m_Waypoints;

    public Tile[,] m_Map;

    [SerializeField]
    private Tile[] m_SerializableMap;
    [SerializeField]
    private int m_SerializableMapWidth;
    [SerializeField]
    private int m_SerializableMapHeight;

    public void OnBeforeSerialize()
    {
        if (m_Map != null && m_Map.Length > 0)
        {
            // Convert the map into a flat array
            m_SerializableMap = new Tile[m_Map.Length];
            m_SerializableMapHeight = m_Map.GetLength(0);
            m_SerializableMapWidth = m_Map.GetLength(1);
                
            for (int row = 0; row < m_Map.GetLength(0); row++)
            {
                for (int col = 0; col < m_Map.GetLength(1); col++)
                {
                    m_SerializableMap[row * m_Map.GetLength(1) + col] = m_Map[row, col];
                }
            }
        }
        else
        {
            m_SerializableMap = null;
        }
    }

    public void OnAfterDeserialize()
    {
        if (m_SerializableMap != null && m_SerializableMap.Length > 0)
        {
            m_Map = new Tile[m_SerializableMapHeight, m_SerializableMapWidth];

            for (int row = 0; row < m_Map.GetLength(0); row++)
            {
                for (int col = 0; col < m_Map.GetLength(1); col++)
                {
                    m_Map[row, col] = m_SerializableMap[row * m_Map.GetLength(1) + col];
                }
            }
        }
        else
        {
            m_Map = null;
        }
    }
}
