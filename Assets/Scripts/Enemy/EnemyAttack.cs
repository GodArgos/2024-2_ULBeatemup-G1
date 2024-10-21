using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private float m_AttackRange = 0.25f;

    private Transform m_AttackQueryPoint;

    [SerializeField]
    private EnemyMovement m_EnemyMovement;
    [SerializeField]
    private Transform m_ThrowablePoint;
    [SerializeField]
    private GameObject m_ShurikenPrefab;

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

    public void GenerateShuriken()
    {
        GameObject shurikkenInstance = Instantiate(m_ShurikenPrefab, m_ThrowablePoint.position, Quaternion.identity);
        MoveShurikken(shurikkenInstance); // Mueve el shuriken
    }

    private void MoveShurikken(GameObject shurikkenInstance)
    {
        // Obtener el Rigidbody2D del shuriken
        Rigidbody2D shurikkenIns = shurikkenInstance.GetComponent<Rigidbody2D>();

        // Desactivar la gravedad para que el shuriken no caiga
        shurikkenIns.gravityScale = 0f;

        // Determinar la dirección basada en la posición del jugador
        Vector2 direction = m_EnemyMovement.m_PlayerHitbox.bounds.center.x < m_EnemyMovement.transform.position.x ? Vector2.left : Vector2.right;

        // Ajustar la fuerza del shuriken
        float launchForce = 20f;

        // Aplicar la fuerza instantáneamente en la dirección correcta
        shurikkenIns.AddForce(direction * launchForce, ForceMode2D.Impulse);
    }
}
