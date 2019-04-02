using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TowerAttack : Buff, ITowerAttackBuff
{
    public GameObject m_Projectile;

    public abstract void Attack(Enemy[] targetsInRange, Tower tower);
}
