using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle, Chasing, AttackMelee, AttackRange
}

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float m_RaycastDistance = 3f;
    [SerializeField]
    private float m_AttackMeleeDistance = 1.5f;
    [SerializeField]
    private float m_AttackRangeDistance = 5.0f;
    [SerializeField]
    private float m_Speed = 4f;
    [SerializeField]
    private Transform m_RaycastGenerator;
    private EnemyState m_State = EnemyState.Idle;
    private Animator m_SpriteAnimator;
    private bool m_IsTalking = false;
    [SerializeField] private Collider2D m_objectCollider;
    private Collider2D m_PlayerHitbox = null;

    private void Awake() 
    {
        m_SpriteAnimator = transform.Find("Sprite").GetComponent<Animator>();
        //m_RaycastGenerator = transform.Find("RaycastGenerator");
    }

    private void Update()
    {
        // Solo si no tenemos al jugador, lo detectamos
        if (m_PlayerHitbox == null)
        {
            DetectPlayerHitbox();
        }

        // Si ya tenemos un jugador detectado
        if (m_PlayerHitbox != null)
        {
            float distance = GetPlayerDistanceToHitboxCenter();

            if (distance > 0f)
            {
                AttackorChase(distance);
            }
            else
            {
                m_State = EnemyState.Idle;
            }

            switch (m_State)
            {
                case EnemyState.Idle:
                    OnIdle();
                    break;
                case EnemyState.Chasing:
                    OnChase();
                    break;
                case EnemyState.AttackMelee:
                    OnAttackMelee();
                    break;
                case EnemyState.AttackRange:
                    OnAttackRange();
                    break;
            }
        }
    }

    private void OnIdle()
    {
        m_SpriteAnimator.SetTrigger("Stop");
    }

    private void OnChase()
    {
        if (m_PlayerHitbox == null) // Evitar movimiento mientras está cargando
        {
            return;
        }

        // Moverse hacia el centro del collider de la hitbox
        Vector3 hitboxCenter = m_PlayerHitbox.bounds.center;
        Vector3 dir = (hitboxCenter - m_objectCollider.bounds.center).normalized;

        // Ajustar la dirección del sprite antes de moverse
        FlipSprite(hitboxCenter);

        transform.position += m_Speed * Time.deltaTime * dir;
        m_SpriteAnimator.SetTrigger("StartWalk");
    }

    private void OnAttackMelee()
    {
        m_SpriteAnimator.SetTrigger("Stop");
        m_SpriteAnimator.SetTrigger("MeleeAttack");
    }

    private void OnAttackRange()
    {
        m_SpriteAnimator.SetTrigger("Stop");
        m_SpriteAnimator.SetTrigger("RangeAttack");
    }

    private void AttackorChase(float distance)
    {
        if (distance < m_AttackMeleeDistance)
        {
            m_State = EnemyState.AttackMelee;
        }
        else if (distance < m_AttackRangeDistance)
        {
            m_State = EnemyState.AttackRange;
        }
        else
        {
            m_State = EnemyState.Chasing;
        }

    }

    private float GetPlayerDistanceToHitboxCenter()
    {
        if (m_PlayerHitbox != null)
        {
            // Calcula la distancia al centro del bounds del collider de la hitbox
            return Vector3.Distance(m_PlayerHitbox.bounds.center, m_objectCollider.bounds.center);
        }

        return -1f;
    }

    private void DetectPlayerHitbox()
    {
        // Detecta los colliders en la capa Hitbox dentro del radio
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
            m_RaycastGenerator.position,
            m_RaycastDistance,
            LayerMask.GetMask("Hitbox")
        );

        if (hitColliders.Length > 0)
        {
            // Asignar el primer hitbox detectado a m_PlayerHitbox
            m_PlayerHitbox = hitColliders[0];
            Debug.Log("Hitbox del jugador detectada.");
        }
        else
        {
            m_PlayerHitbox = null; // No encontró al jugador
        }
    }

    private void FlipSprite(Vector3 hitboxCenter)
    {
        // Si el jugador está a la derecha del enemigo
        if (hitboxCenter.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Girar a la derecha
        }
        // Si el jugador está a la izquierda del enemigo
        else if (hitboxCenter.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1); // Girar a la izquierda
        }
    }

    public void Talk()
    {
        if (!m_IsTalking)
        {
            m_SpriteAnimator.SetTrigger("Talk");
            m_IsTalking = true;
        }else
        {
            m_SpriteAnimator.SetTrigger("StopTalk");
            m_IsTalking = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_RaycastGenerator.position, m_RaycastDistance);

        if (m_PlayerHitbox != null)
        {
            // Dibuja líneas para representar las distancias en los ejes X y Y
            Gizmos.color = Color.red;
            Gizmos.DrawLine(m_objectCollider.bounds.center, m_PlayerHitbox.bounds.center);
        }
    }
}
