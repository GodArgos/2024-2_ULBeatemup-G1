using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField]
    private float m_RaycastDistance = 3f;
    [SerializeField]
    private float m_AttackDistance = 0.5f;
    [SerializeField]
    private float m_Speed = 4f;
    [SerializeField]
    private Transform m_RaycastGenerator;

    private EnemyState m_State = EnemyState.Idle;
    private Animator m_SpriteAnimator;
    private bool m_IsTalking = false;

    private Collider2D m_PlayerHitbox = null;

    [SerializeField]
    private float m_ChargeMinTime = 1f;
    [SerializeField]
    private float m_ChargeMaxTime = 5f;
    private bool m_IsCharging = false;
    public bool IsCharging
    {
        get
        {
            return m_IsCharging;
        }
        set
        {
            m_IsCharging = value;
        }
    }

    private Vector3 m_ChargePosition;

    // Cooldown para el ataque cargado
    private float m_ChargeCooldown;
    private float m_NextChargeAttackTime;

    private void Awake()
    {
        m_SpriteAnimator = transform.Find("Sprite").GetComponent<Animator>();
        // Establecer el cooldown inicial a 0
        m_ChargeCooldown = 0f;
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
                case EnemyState.Attacking:
                    OnAttack();
                    break;
            }
        }

        // Verifica el cooldown
        if (Time.time > m_NextChargeAttackTime)
        {
            m_ChargeCooldown = 0f; // El cooldown se ha completado
        }
    }

    private void OnIdle()
    {
        m_SpriteAnimator.SetTrigger("Stop");
    }

    private void OnChase()
    {
        if (m_IsCharging || m_PlayerHitbox == null) // Evitar movimiento mientras está cargando
        {
            return;
        }

        // Moverse hacia el centro del collider de la hitbox
        Vector3 hitboxCenter = m_PlayerHitbox.bounds.center;
        Vector3 dir = (hitboxCenter - transform.position).normalized;

        // Ajustar la dirección del sprite antes de moverse
        FlipSprite(hitboxCenter);

        transform.position += m_Speed * Time.deltaTime * dir;
        m_SpriteAnimator.SetTrigger("StartWalk");
    }

    private void OnAttack()
    {
        // Aquí es donde se manejarían los diferentes tipos de ataque
        if (m_IsCharging)
        {
            // Si está en carga, no hacemos nada
            return;
        }

        // Realizar ataque débil
        Debug.Log("Realizando ataque débil.");
        m_SpriteAnimator.SetTrigger("Stop");
        m_SpriteAnimator.SetTrigger("Attack");
    }

    private void AttackorChase(float distance)
    {
        // Solo se permite el ataque cargado si no está en cooldown
        if (!m_IsCharging && m_ChargeCooldown <= 0 && Random.Range(0, 5) == 0) // 20% de probabilidad
        {
            StartCoroutine(ChargeAttack());
        }
        else if (distance < m_AttackDistance)
        {
            m_State = EnemyState.Attacking;
        }
        else
        {
            m_State = EnemyState.Chasing;
        }
    }

    private IEnumerator ChargeAttack()
    {
        m_IsCharging = true;
        m_SpriteAnimator.SetTrigger("Charging");

        // Tiempo de carga aleatorio
        float chargeTime = Random.Range(m_ChargeMinTime, m_ChargeMaxTime);

        // Mientras carga, girar hacia el jugador
        while (chargeTime > 0)
        {
            FlipSprite(m_PlayerHitbox.bounds.center); // Girar hacia el jugador
            yield return null; // Esperar el siguiente frame
            chargeTime -= Time.deltaTime; // Decrementar el tiempo de carga
        }

        // Guardar posición actual del jugador
        m_ChargePosition = m_PlayerHitbox.bounds.center;

        // Cambiar al ataque cargado
        m_SpriteAnimator.SetTrigger("ChargingAttack");

        // Moverse rápidamente hacia la posición del jugador
        float chargeSpeed = 10f; // Puedes ajustar esta velocidad
        while (Vector3.Distance(transform.position, m_ChargePosition) > m_AttackDistance)
        {
            Vector3 dir = (m_ChargePosition - transform.position).normalized;
            transform.position += chargeSpeed * Time.deltaTime * dir;
            yield return null; // Esperar el siguiente frame
        }

        // Aquí se puede implementar el daño al jugador si lo alcanza
        if (Vector3.Distance(transform.position, m_ChargePosition) <= m_AttackDistance)
        {
            //m_PlayerHitbox.GetComponent<EnemyHitbox>().Hit(); // Asegúrate de que el script está en el hitbox
            Debug.Log("Ataque cargado exitoso.");
        }
        else
        {
            Debug.Log("El ataque cargado falló.");
        }

        // Calcular y establecer el cooldown
        m_ChargeCooldown = Random.Range(5f, 7f);
        m_NextChargeAttackTime = Time.time + m_ChargeCooldown;

        // Regresar al estado idle
        m_State = EnemyState.Idle;
        m_IsCharging = false;
    }

    private float GetPlayerDistanceToHitboxCenter()
    {
        if (m_PlayerHitbox != null)
        {
            // Calcula la distancia al centro del bounds del collider de la hitbox
            return Vector3.Distance(m_PlayerHitbox.bounds.center, transform.position);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_RaycastGenerator.position, m_RaycastDistance);
    }
}
