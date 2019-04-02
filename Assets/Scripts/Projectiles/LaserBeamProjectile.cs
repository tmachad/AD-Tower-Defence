using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamProjectile : Projectile {

    private SpriteRenderer m_SpriteRenderer;

    private new void Awake()
    {
        base.Awake();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private new void Start()
    {
        base.Start();
        float distance = Vector2.Distance(transform.position, m_TargetPosition) * 2;    // Position is halfway between tower and target, so multiply by 2
        m_SpriteRenderer.size = new Vector2(distance, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerMask.GetMask(LayerMask.LayerToName(other.gameObject.layer)) == m_CollidesWith)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(m_Damage);

                if (m_OnHit != null)
                {
                    m_OnHit(enemy.gameObject, this);
                }
            }
        }
    }
}
