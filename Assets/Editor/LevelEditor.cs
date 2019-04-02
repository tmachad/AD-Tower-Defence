using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Linq;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor {

    private Level m_TargetLevel;

    private SerializedProperty m_PathTileProperty;
    private SerializedProperty m_BuildableTileProperty;
    private SerializedProperty m_SpawnerTileProperty;
    private SerializedProperty m_GoalTileProperty;
    private SerializedProperty m_WidthProperty;
    private SerializedProperty m_HeightProperty;
    private SerializedProperty m_WaypointsProperty;
    private SerializedProperty m_SerializableMapProperty;
    private SerializedProperty m_PreviewSpriteProperty;

    private ReorderableList m_WaypointsList;

    private int m_SelectedTileToPlace = 0;
    private string[] m_TileNames;

    private Vector2 m_WaypointListScrollPosition;
    private Vector2 m_MapScrollPosition;

    private int m_TileSize = 25;
    private int m_PreviewBuffer = 50;

    private int m_MouseRow = 0;
    private int m_MouseCol = 0;

    private bool m_SpawnerPlaced = false;
    private bool m_GoalPlaced = false;

    private void OnEnable()
    {
        m_TargetLevel = (Level)target;

        m_PathTileProperty = serializedObject.FindProperty("m_PathTile");
        m_BuildableTileProperty = serializedObject.FindProperty("m_BuildableTile");
        m_SpawnerTileProperty = serializedObject.FindProperty("m_SpawnerTile");
        m_GoalTileProperty = serializedObject.FindProperty("m_GoalTile");
        m_WidthProperty = serializedObject.FindProperty("m_Width");
        m_HeightProperty = serializedObject.FindProperty("m_Height");
        m_WaypointsProperty = serializedObject.FindProperty("m_Waypoints");
        m_SerializableMapProperty = serializedObject.FindProperty("m_SerializableMap");
        m_PreviewSpriteProperty = serializedObject.FindProperty("m_PreviewSprite");

        m_WaypointsList = new ReorderableList(serializedObject, m_WaypointsProperty, true, true, true, true);
        m_WaypointsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            SerializedProperty element = m_WaypointsList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
        };
        m_WaypointsList.drawHeaderCallback = (Rect rect) => 
        {
            EditorGUI.LabelField(rect, m_WaypointsList.serializedProperty.displayName);
        };
        m_WaypointsList.onCanRemoveCallback = (ReorderableList list) =>
        {
            // Disable the remove button for the first and last elements in the list
            return list.index != 0 && list.index != list.count - 1;
        };
        m_WaypointsList.onAddCallback = (ReorderableList list) =>
        {
            list.serializedProperty.InsertArrayElementAtIndex(list.count - 2);
        };
        m_WaypointsList.onReorderCallbackWithDetails = (ReorderableList list, int oldIndex, int newIndex) =>
        {
            if (oldIndex == 0 || oldIndex == list.count - 1 || newIndex == 0 || newIndex == list.count - 1)
            {
                // Someone tried to move the spawner or goal waypoints or move something else into their place, so move back
                list.serializedProperty.MoveArrayElement(newIndex, oldIndex);
            }
        };


        List<string> tileNames = new List<string>();
        foreach (Level.Tile tile in Enum.GetValues(typeof(Level.Tile)))
        {
            tileNames.Add(tile.ToString());
        }
        m_TileNames = tileNames.ToArray();

        if (m_TargetLevel.m_Map == null)
        {
            // There isn't already a map, so create one and initialize it to empty
            m_TargetLevel.m_Map = new Level.Tile[m_TargetLevel.m_Height, m_TargetLevel.m_Width];

            for (int row = 0; row < m_TargetLevel.m_Map.GetLength(0); row++)
            {
                for (int col = 0; col < m_TargetLevel.m_Map.GetLength(1); col++)
                {
                    m_TargetLevel.m_Map[row, col] = Level.Tile.Empty;
                }
            }
        }

        // Check if there's already a spawner and/or goal placed
        m_SpawnerPlaced = false;
        m_GoalPlaced = false;
        for (int row = 0; row < m_TargetLevel.m_Map.GetLength(0); row++)
        {
            for (int col = 0; col < m_TargetLevel.m_Map.GetLength(1); col++)
            {
                if (m_TargetLevel.m_Map[row, col] == Level.Tile.Spawner)
                {
                    m_SpawnerPlaced = true;
                }
                else if (m_TargetLevel.m_Map[row, col] == Level.Tile.Goal)
                {
                    m_GoalPlaced = true;
                }
            }
        }

        if (m_TargetLevel.m_Waypoints == null)
        {
            m_TargetLevel.m_Waypoints = new List<Vector2Int>();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_PreviewSpriteProperty);
        EditorGUILayout.PropertyField(m_PathTileProperty);
        EditorGUILayout.PropertyField(m_BuildableTileProperty);
        EditorGUILayout.PropertyField(m_SpawnerTileProperty);
        EditorGUILayout.PropertyField(m_GoalTileProperty);
        EditorGUILayout.PropertyField(m_HeightProperty);
        EditorGUILayout.PropertyField(m_WidthProperty);
        

        if (m_HeightProperty.intValue != m_TargetLevel.m_Map.GetLength(0) || m_WidthProperty.intValue != m_TargetLevel.m_Map.GetLength(1))
        {
            // Height or width changed, so provide a button to update the map
            if (GUILayout.Button("Update Map Dimensions"))
            {
                Level.Tile[,] newMap = new Level.Tile[m_HeightProperty.intValue, m_WidthProperty.intValue];

                // Copy what can be copied from old map, and either discard excess or fill new space with empty tiles
                for (int row = 0; row < newMap.GetLength(0); row++)
                {
                    for (int col = 0; col < newMap.GetLength(1); col++)
                    {
                        if (row < m_TargetLevel.m_Map.GetLength(0) && col < m_TargetLevel.m_Map.GetLength(1))
                        {
                            // This square is within the old map bounds, so just copy the old value
                            newMap[row, col] = m_TargetLevel.m_Map[row, col];
                        }
                        else
                        {
                            // This square is outside the old map bounds, so fill with empty
                            newMap[row, col] = Level.Tile.Empty;
                        }
                    }
                }

                m_TargetLevel.m_Map = newMap;
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Map (" + m_TargetLevel.m_Map.GetLength(0) + "x" + m_TargetLevel.m_Map.GetLength(1) + ")", EditorStyles.boldLabel);

        m_WaypointListScrollPosition = EditorGUILayout.BeginScrollView(m_WaypointListScrollPosition);
        m_WaypointsList.DoLayoutList();
        EditorGUILayout.EndScrollView();

        EditorGUILayout.LabelField("Place");
        m_SelectedTileToPlace = GUILayout.SelectionGrid(m_SelectedTileToPlace, m_TileNames, m_TileNames.Length);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Mouse Position", "Col(X): " + m_MouseCol + ", Row(Y): " + m_MouseRow);
        m_MapScrollPosition = EditorGUILayout.BeginScrollView(m_MapScrollPosition);

        GUILayoutUtility.GetRect(
            m_PreviewBuffer * 2 + m_TileSize * m_TargetLevel.m_Map.GetLength(1) + m_TargetLevel.m_Map.GetLength(1), 
            m_PreviewBuffer * 2 + m_TileSize * m_TargetLevel.m_Map.GetLength(0) + m_TargetLevel.m_Map.GetLength(0)
        );

        for (int row = 0; row < m_TargetLevel.m_Map.GetLength(0); row++)
        {
            for (int col = 0; col < m_TargetLevel.m_Map.GetLength(1); col++)
            {
                Rect rect = new Rect(
                    m_PreviewBuffer + m_TileSize * col + col,
                    m_PreviewBuffer + m_TileSize * row + row,
                    m_TileSize,
                    m_TileSize
                    );

                if (Event.current.isMouse && rect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.button == 0)
                    {
                        // The left mouse button was clicked, so change this tile to the currently selected tile
                        Level.Tile newTile = (Level.Tile)m_SelectedTileToPlace;

                        if (newTile == Level.Tile.Spawner || newTile == Level.Tile.Goal)
                        {
                            // Attempting to place a spawner or goal, so remove any pre-existing ones
                            for (int r = 0; r < m_TargetLevel.m_Map.GetLength(0); r++)
                            {
                                for (int c = 0; c < m_TargetLevel.m_Map.GetLength(1); c++)
                                {
                                    if (m_TargetLevel.m_Map[r, c] == newTile)
                                    {
                                        // Found a pre-existing spawner or goal, so remove it
                                        m_TargetLevel.m_Map[r, c] = Level.Tile.Empty;
                                    }
                                }
                            }

                            if (newTile == Level.Tile.Spawner)
                            {
                                // Move first waypoint to new spawner location

                                if (!m_SpawnerPlaced) {
                                    
                                    if (m_WaypointsProperty.arraySize < 2)
                                    {
                                        // There's less than 2 waypoints, so add a new one for the spawner
                                        m_WaypointsProperty.InsertArrayElementAtIndex(0);
                                    }
                                    
                                    m_SpawnerPlaced = true;
                                }

                                SerializedProperty spawnerWaypoint = m_WaypointsProperty.GetArrayElementAtIndex(0);
                                spawnerWaypoint.vector2IntValue = new Vector2Int(col, row);
                            }
                            else if (newTile == Level.Tile.Goal)
                            {
                                // Move last waypoint to new goal location
                                if (!m_GoalPlaced)
                                {
                                    if (m_WaypointsProperty.arraySize < 2)
                                    {
                                        // There's less than 2 waypoints, so add a new one for the goal
                                        m_WaypointsProperty.InsertArrayElementAtIndex(m_WaypointsProperty.arraySize - 1);
                                    }
                                    
                                    m_GoalPlaced = true;
                                }
                                SerializedProperty goalWaypoint = m_WaypointsProperty.GetArrayElementAtIndex(m_WaypointsProperty.arraySize - 1);
                                goalWaypoint.vector2IntValue = new Vector2Int(col, row);
                            }
                        }

                        m_TargetLevel.m_Map[row, col] = newTile;
                        // Also need to update serialized property for serializablemap, otherwise all changes will be lost on restart for some reason...
                        m_SerializableMapProperty.GetArrayElementAtIndex(row * m_TargetLevel.m_Map.GetLength(1) + col).enumValueIndex = (int)newTile;
                    }
                    else if (Event.current.button == 1)
                    {
                        // The right mouse button was clicked, so replace this tile with empty
                        m_TargetLevel.m_Map[row, col] = Level.Tile.Empty;
                    }
                }

                if (rect.Contains(Event.current.mousePosition))
                {
                    m_MouseRow = row;
                    m_MouseCol = col;
                }

                EditorGUI.DrawRect(rect, 
                    GetTileColor(m_TargetLevel.m_Map[row, col])
                );
            }
        }

        Handles.BeginGUI();
        Handles.color = Color.yellow;
        List<Vector3> path = new List<Vector3>();
        foreach (Vector2Int point in m_TargetLevel.m_Waypoints)
        {
            path.Add(new Vector3(
                    m_PreviewBuffer + m_TileSize * point.x + point.x + m_TileSize / 2,
                    m_PreviewBuffer + m_TileSize * point.y + point.y + m_TileSize / 2
                )
            );
            EditorGUI.DrawRect(new Rect(
                m_PreviewBuffer + m_TileSize * point.x + point.x + m_TileSize / 2 - m_TileSize / 10,
                m_PreviewBuffer + m_TileSize * point.y + point.y + m_TileSize / 2 - m_TileSize / 10,
                m_TileSize / 5,
                m_TileSize / 5),
                Color.yellow
           );
        }
        Handles.DrawPolyLine(path.ToArray());
        Handles.EndGUI();

        EditorGUILayout.EndScrollView();

        serializedObject.ApplyModifiedProperties();

        Repaint();
    }

    private Color GetTileColor(Level.Tile tile)
    {
        switch (tile)
        {
            case Level.Tile.Path:
                return new Color(0.827f, 0.827f, 0.827f);    // Light grey
            case Level.Tile.Buildable:
                return Color.grey;
            case Level.Tile.Spawner:
                return Color.red;
            case Level.Tile.Goal:
                return Color.green;
            case Level.Tile.Empty:
                return Color.black;
            default:
                return Color.magenta;
        }
    }
}
