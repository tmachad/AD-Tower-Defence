using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour {

    public enum TargetPriority
    {
        First,
        Last,
        Strongest,
        Weakest,
        None
    }

    public float m_BaseRange = 3.0f;
    public float m_Range
    {
        get {
            float range = m_BaseRange;

            foreach(Buff buff in m_Buffs)
            {
                if (buff is ITowerStatBuff)
                {
                    range = ((ITowerStatBuff)buff).ApplyRangeBuff(range);
                }
            }

            return range;
        }
    }

    public float m_BaseFireRate = 1.5f;
    public float m_FireRate
    {
        get
        {
            float fireRate = m_BaseFireRate;

            foreach (Buff buff in m_Buffs)
            {
                if (buff is ITowerStatBuff)
                {
                    fireRate = ((ITowerStatBuff)buff).ApplyFireRateBuff(fireRate);
                }
            }

            return fireRate;
        }
    }

    public float m_BaseDamage = 1.0f;
    public float m_Damage
    {
        get
        {
            float damage = m_BaseDamage;

            foreach (Buff buff in m_Buffs)
            {
                if (buff is ITowerStatBuff)
                {
                    damage = ((ITowerStatBuff)buff).ApplyDamageBuff(damage);
                }
            }

            return damage;
        }
    }

    public float m_BaseCost = 10.0f;
    public float m_Cost
    {
        get
        {
            float cost = m_BaseCost;

            foreach (Buff buff in m_Buffs)
            {
                if (buff != null)
                {
                    cost += buff.m_Cost;
                }
            }

            return cost;
        }
    }

    public TargetPriority m_TargetPriority;

    public Buff[] m_Buffs;

    public Tower m_Template;

    [SerializeField]
    private BasicTowerAttack m_DefaultAttack;

    private float m_FireDelay;

    private const int m_NumberOfRangePreviewPoints = 50;
    private LineRenderer m_RangeIndicator;

    private void Awake()
    {
        // Clone default attack asset to avoid modifying original
        m_DefaultAttack = Instantiate(m_DefaultAttack);

        for (int i = 0; i < m_Buffs.Length; i++)
        {
            // Clone any existing buffs to avoid modifiying original
            if (m_Buffs[i] != null)
            {
                m_Buffs[i] = Instantiate(m_Buffs[i]);
            }
        }

        m_RangeIndicator = GetComponent<LineRenderer>();
        m_RangeIndicator.positionCount = m_NumberOfRangePreviewPoints;
        m_RangeIndicator.enabled = false;
    }

    private void Start()
    {
        m_FireDelay = 1.0f / m_FireRate;
    }

    private void Update()
    {
        m_FireDelay -= Time.deltaTime;

        if (m_FireDelay <= 0)
        {
            // It's time to fire again
            Enemy[] targetsInRange = EnemyManager.Instance.m_Enemies
                                            .Where(e => Vector2.Distance(e.gameObject.transform.position, transform.position) <= m_Range)
                                            .ToArray();
            targetsInRange = SortTargets(targetsInRange);

            if (targetsInRange.Length > 0)
            {
                bool attacked = false;

                foreach (Buff buff in m_Buffs)
                {
                    if (buff is ITowerAttackBuff)
                    {
                        attacked = true;
                        ((ITowerAttackBuff)buff).Attack(targetsInRange, this);
                    }
                }

                if (!attacked)
                {
                    // This tower doesn't have any attack buffs equiped, use default attack then
                    m_DefaultAttack.Attack(targetsInRange, this);
                }

                foreach (Buff buff in m_Buffs)
                {
                    if (buff is ITowerOnAttackBuff)
                    {
                        ((ITowerOnAttackBuff)buff).OnAttack();
                    }
                }

                m_FireDelay = 1.0f / m_FireRate;
            }
        }
    }

    private Enemy[] SortTargets(Enemy[] targetsInRange)
    {
        List<Enemy> targets = new List<Enemy>(targetsInRange);

        if (m_TargetPriority == TargetPriority.First)
        {
            targets.Sort((e1, e2) =>
            {
                float d1 = e1.DistanceToGoal();
                float d2 = e2.DistanceToGoal();

                if (d1 < d2)
                {
                    return -1;
                }
                else if (d1 > d2)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });
        }
        else if (m_TargetPriority == TargetPriority.Last)
        {
            targets.Sort((e1, e2) =>
            {
                float d1 = e1.DistanceToGoal();
                float d2 = e2.DistanceToGoal();

                if (d1 < d2)
                {
                    return 1;
                }
                else if (d1 > d2)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            });
        }
        else if (m_TargetPriority == TargetPriority.Strongest)
        {
            targets.Sort((e1, e2) =>
            {
                if (e1.m_CurrentHealth < e2.m_CurrentHealth)
                {
                    return 1;
                }
                else if (e1.m_CurrentHealth > e2.m_CurrentHealth)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            });
        }
        else if (m_TargetPriority == TargetPriority.Weakest)
        {
            targets.Sort((e1, e2) =>
            {
                if (e1.m_CurrentHealth < e2.m_CurrentHealth)
                {
                    return -1;
                }
                else if (e1.m_CurrentHealth > e2.m_CurrentHealth)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });
        }
        else if (m_TargetPriority == TargetPriority.None)
        {
            targets.Clear();
        }

        return targets.ToArray();
    }

    public void ShowRangeIndicator(bool show)
    {
        if (show)
        {
            m_RangeIndicator.SetPositions(GetRangePreviewPoints());
        }
        m_RangeIndicator.enabled = show;
    }

    public Vector3[] GetRangePreviewPoints()
    {
        Vector3[] rangePreviewPoints = new Vector3[m_NumberOfRangePreviewPoints];
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < m_NumberOfRangePreviewPoints; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * m_Range;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * m_Range;

            rangePreviewPoints[i] = new Vector3(x, y, 0);

            angle += 360f / m_NumberOfRangePreviewPoints;
        }

        return rangePreviewPoints;
    }
}
