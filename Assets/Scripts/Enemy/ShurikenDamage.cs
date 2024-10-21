using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenDamage : MonoBehaviour
{
    [SerializeField]
    private float m_AttackRange = 0.25f;

    private void Update()
    {
        AttackQuery();
    }

    public void AttackQuery()
    {
        var collider = Physics2D.OverlapCircle(
            transform.position,
            m_AttackRange,
            LayerMask.GetMask("Hitbox")
        );
        if (collider != null)
        {
            // hubo colision
            Debug.Log("Hubo golpe del enemigo");
            collider.gameObject.GetComponent<IHitbox>().Hit(Random.Range(10f, 15f));
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, m_AttackRange);
    }
}
