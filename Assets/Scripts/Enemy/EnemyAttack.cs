using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private float m_AttackRange = 0.25f;

    private Transform m_AttackQueryPoint;

    private void Awake()
    {
        m_AttackQueryPoint = transform.Find("AttackQueryPoint");
    }

    public void AttackQuery()
    {
        var collider = Physics2D.OverlapCircle(
            m_AttackQueryPoint.position,
            m_AttackRange,
            LayerMask.GetMask("Hitbox")
        );
        if (collider != null)
        {
            // hubo colision
            Debug.Log("Hubo golpe del enemigo");
            collider.gameObject.GetComponent<IHitbox>().Hit(Random.Range(10f, 15f));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(m_AttackQueryPoint.position, m_AttackRange);
    }
}
