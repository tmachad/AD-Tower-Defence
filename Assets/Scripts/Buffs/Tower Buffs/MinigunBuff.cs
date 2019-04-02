using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower Buffs/Minigun")]
public class MinigunBuff : Buff, ITowerStatBuff, ITowerOnAttackBuff
{
    public float m_FireRateRampIncrement = 1f;
    public float m_MaxFireRateBonus = 5f;
    public float m_FireRateDecayPerSecond = 0.5f;

    private float m_LastAttackTime = 0f;
    private float m_BonusFireRate = 0f;

    public float ApplyDamageBuff(float damage)
    {
        return damage;
    }

    public float ApplyFireRateBuff(float fireRate)
    {
        return fireRate + m_BonusFireRate;
    }

    public float ApplyRangeBuff(float range)
    {
        return range;
    }

    public void OnAttack()
    {
        m_BonusFireRate += m_FireRateRampIncrement - (Time.time - m_LastAttackTime) * m_FireRateDecayPerSecond;
        m_BonusFireRate = Mathf.Clamp(m_BonusFireRate, 0, m_MaxFireRateBonus);
        m_LastAttackTime = Time.time;
    }
}
