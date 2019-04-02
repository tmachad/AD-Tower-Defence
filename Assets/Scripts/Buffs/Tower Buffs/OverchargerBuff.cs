using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower Buffs/Overcharger")]
public class OverchargerBuff : Buff, ITowerStatBuff, ITowerOnAttackBuff
{
    public float m_DamageRampPerSecond = 1f;

    private float m_LastAttackTime;

    private void OnEnable()
    {
        m_LastAttackTime = Time.time;
    }

    public float ApplyDamageBuff(float damage)
    {
        return damage + m_DamageRampPerSecond * (Time.time - m_LastAttackTime);
    }

    public float ApplyFireRateBuff(float fireRate)
    {
        return fireRate;
    }

    public float ApplyRangeBuff(float range)
    {
        return range;
    }

    public void OnAttack()
    {
        m_LastAttackTime = Time.time;
    }
}
