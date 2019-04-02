using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower Buffs/Sniper Buff")]
public class SniperBuff : Buff, ITowerStatBuff
{
    public float m_DamageMultiplier;
    public float m_FireRateMultiplier;
    public float m_RangeMultiplier;

    public float ApplyDamageBuff(float damage)
    {
        return damage * m_DamageMultiplier;
    }

    public float ApplyFireRateBuff(float fireRate)
    {
        return fireRate * m_FireRateMultiplier;
    }

    public float ApplyRangeBuff(float range)
    {
        return range * m_RangeMultiplier;
    }
}
