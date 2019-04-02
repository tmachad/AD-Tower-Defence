using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Tower Buffs/Basic Tower Attack")]
public class BasicTowerAttack : TowerAttack
{
    public override void Attack(Enemy[] targetsInRange, Tower tower)
    {
        Enemy target = targetsInRange[0];

        GameObject projectileObj = Instantiate(m_Projectile, tower.transform);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        
        foreach(Buff buff in tower.m_Buffs)
        {
            if (buff is ITowerOnHitBuff)
            {
                projectile.m_OnHit += ((ITowerOnHitBuff)buff).OnHit;
            }
        }

        projectile.m_Target = target;
        Vector3 pos = tower.transform.position;
        pos.z -= 2; // Move closer to the camera so it's visible above towers and enemies
        projectile.transform.position = pos;
        projectile.m_Damage = tower.m_Damage;
    }
}
