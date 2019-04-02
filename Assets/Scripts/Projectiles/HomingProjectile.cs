using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Projectile {

    public bool m_OnlyHitTarget = true;

    private void Update()
    {
        if (m_Target != null)
        {
            // Since the target is still alive, update target position to their position
            m_TargetPosition = m_Target.transform.position;
        }

        //Debug.Log("Distance to target: " + Vector2.Distance(m_TargetPosition, transform.position));
        if (Vector2.Distance(m_TargetPosition, transform.position) <= 0.01)
        {
            // If we've reached the target position without hitting a target, destroy myself
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Move();
        FaceTarget();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerMask.GetMask(LayerMask.LayerToName(other.gameObject.layer)) == m_CollidesWith)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (m_OnlyHitTarget && enemy == m_Target)
            {
                enemy.TakeDamage(m_Damage);
                
                if (m_OnHit != null)
                {
                    m_OnHit(enemy.gameObject, this);
                }

                Destroy(gameObject);
            }
            else if (!m_OnlyHitTarget)
            {
                enemy.TakeDamage(m_Damage);

                if (m_OnHit != null)
                {
                    m_OnHit(enemy.gameObject, this);
                }

                Destroy(gameObject);
            }
        }
    }
}
