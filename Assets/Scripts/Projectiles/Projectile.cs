using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {

    public delegate void OnHItDelegate(GameObject hit, Projectile projectile);

    [HideInInspector]
    public Enemy m_Target;
    public float m_MoveSpeed = 3.0f;
    public float m_TurnSpeed = 180.0f;
    [HideInInspector]
    public float m_Damage;
    public LayerMask m_CollidesWith;

    public OnHItDelegate m_OnHit;

    protected Rigidbody2D m_Rigidbody;
    protected Vector3 m_TargetPosition;

    protected void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        m_TargetPosition = m_Target.transform.position;

        m_Rigidbody.isKinematic = true;

        Vector3 vectorToTarget = m_TargetPosition - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 360);

        m_Rigidbody.isKinematic = false;
    }

    protected void Move()
    {
        m_Rigidbody.MovePosition(Vector2.MoveTowards(transform.position, m_TargetPosition, m_MoveSpeed * Time.fixedDeltaTime));
    }

    protected void FaceTarget()
    {
        Vector3 vectorToTarget = m_TargetPosition - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        m_Rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, q, Time.fixedDeltaTime * m_TurnSpeed).eulerAngles.z);
    }
}
