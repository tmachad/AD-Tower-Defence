using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public static EnemyManager Instance;

    [HideInInspector]
    public List<Enemy> m_Enemies;

    private void Awake()
    {
        Instance = this;
    }

    public void ResetToInitialState()
    {
        foreach (Enemy enemy in m_Enemies)
        {
            Destroy(enemy.gameObject);
        }

        m_Enemies.Clear();
    }
}
