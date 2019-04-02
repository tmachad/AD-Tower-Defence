using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower Buffs/Explosive Hit Buff")]
public class ExplosiveHitBuff : Buff, ITowerOnHitBuff
{
    public float m_DamageMultiplier;
    public float m_Range;
    public GameObject m_ExplosionPrefab;

    public void OnHit(GameObject hit, Projectile projectile)
    {
        Enemy[] targets = EnemyManager.Instance.m_Enemies.Where(e => Vector2.Distance(hit.transform.position, e.transform.position) <= m_Range).ToArray();

        foreach (Enemy enemy in targets)
        {
            enemy.TakeDamage(projectile.m_Damage * m_DamageMultiplier);
        }

        GameObject explosion = Instantiate(m_ExplosionPrefab);
        explosion.transform.localScale = new Vector3(m_Range * 2, m_Range * 2, 1);
        Vector3 pos = hit.transform.position;
        pos.z -= 0.5f;  // Position explosions above towers and enemies, but below projectiles
        explosion.transform.position = pos;
    }
}
