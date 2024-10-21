using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BossAttack : MonoBehaviour
{
    [SerializeField]
    private BossMovement m_BossMovement;
    [SerializeField]
    private float m_AttackRange = 0.25f;
    private Transform m_AttackQueryPoint;

    private void Awake()
    {
        m_AttackQueryPoint = transform.Find("AttackQueryPoint");
    }

    public void AttackQuery()
    {
        Debug.Log("ME LLAMASTE ASDASDADS");
        var collider = Physics2D.OverlapCircle(
            m_AttackQueryPoint.position,
            m_AttackRange,
            LayerMask.GetMask("Hitbox")
        );
        if (collider != null)
        {
            // hubo colision
            Debug.Log("Hubo golpe del enemigo");
            if (m_BossMovement.IsCharging)
            {
                collider.gameObject.GetComponent<IHitbox>().Hit(Random.Range(30f, 35f));
            }
            else
            {
                collider.gameObject.GetComponent<IHitbox>().Hit(Random.Range(20f, 5f));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(m_AttackQueryPoint.position, m_AttackRange);
    }
}
