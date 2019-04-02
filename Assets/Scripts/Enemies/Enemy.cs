using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public ScalingStat m_Speed;

    public ScalingStat m_MaxHealth;

    public float m_Bounty = 10;

    public Texture2D m_HealthBarTexture;
    public Vector2 m_HealthBarSize;
    public Vector2 m_HealthBarOffset;

    public float m_CurrentHealth { get; private set; }

    private float _m_Speed;
    private float _m_MaxHealth;

    [HideInInspector]
    private Vector2[] m_Waypoints;

    private int m_NextWaypoint;

    private void Start()
    {
        // TODO: Remove enemies from the list when they die
        EnemyManager.Instance.m_Enemies.Add(this);
    }

    private void OnGUI()
    {
        if (m_CurrentHealth < _m_MaxHealth)
        {
            float pxPerMeter = Camera.main.pixelHeight / 2 / Camera.main.orthographicSize;
            Vector3 worldPos = transform.position;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            screenPos.y = Screen.height - screenPos.y;  // Convert from screen space to GUI space
            Rect hpBarRect = new Rect(
                screenPos.x + (m_HealthBarOffset.x * pxPerMeter) - (m_HealthBarSize.x / 2 * pxPerMeter),
                screenPos.y - (m_HealthBarOffset.y * pxPerMeter) - (m_HealthBarSize.y * pxPerMeter),
                m_HealthBarSize.x * pxPerMeter * m_CurrentHealth / _m_MaxHealth,
                m_HealthBarSize.y * pxPerMeter
            );
            GUI.DrawTexture(hpBarRect, m_HealthBarTexture);
        }
    }

    private void Update()
    {
        if (m_NextWaypoint < m_Waypoints.Length)
        {
            // I still have a valid current waypoint

            Vector3 newPos = new Vector3(m_Waypoints[m_NextWaypoint].x, m_Waypoints[m_NextWaypoint].y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPos, _m_Speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, newPos) == 0)
            {
                // Arrived at destination, switch to next waypoint
                m_NextWaypoint++;

                if (m_NextWaypoint == m_Waypoints.Length)
                {
                    // Reached the end of the line
                    GameManager.Instance.EnemyReachedEnd();
                    EnemyManager.Instance.m_Enemies.Remove(this);
                    Destroy(gameObject);
                }
            }
        }
    }

    public void Initialize(Vector3 position, Vector2[] waypoints)
    {
        transform.position = position;

        m_Waypoints = waypoints;

        m_Speed.m_ScalingFactor = WaveManager.Instance;
        m_MaxHealth.m_ScalingFactor = WaveManager.Instance;

        _m_Speed = m_Speed;
        _m_MaxHealth = m_MaxHealth;
        m_CurrentHealth = _m_MaxHealth;
    }

    public float DistanceToGoal()
    {
        if (m_NextWaypoint >= m_Waypoints.Length)
        {
            return 0;
        }
        else
        {
            float distance = Vector2.Distance(transform.position, m_Waypoints[m_NextWaypoint]);

            for (int i = m_NextWaypoint; i < m_Waypoints.Length - 1; i++)
            {
                distance += Vector2.Distance(m_Waypoints[i], m_Waypoints[i + 1]);
            }

            return distance;
        }
    }

    public void TakeDamage(float damage)
    {
        m_CurrentHealth -= damage;

        if (m_CurrentHealth <= 0)
        {
            EnemyManager.Instance.m_Enemies.Remove(this);
            GameManager.Instance.m_Money += m_Bounty;
            Destroy(gameObject);
        }
    }
}
