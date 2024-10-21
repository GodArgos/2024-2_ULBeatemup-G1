using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle, Chasing, AttackingMelee, AttackingRange,
}

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float m_RaycastDistance = 3f;
    [SerializeField]
    private float m_AttackDistanceMelee = 0.5f;
    [SerializeField]
    private float m_AttackDistanceRange = 2f;
    [SerializeField]
    private float m_Speed = 4f;
    [SerializeField]
    private Transform m_RaycastGenerator;
    [SerializeField]
    private float shurikkenCooldown = 1.5f;  // Tiempo entre lanzamientos

    private EnemyState m_State = EnemyState.Idle;
    private Animator m_SpriteAnimator;
    public GameObject shurikken;
    private bool m_IsTalking = false;
    private bool canThrowShurikken = true;  // Controla cuándo el enemigo puede lanzar el shuriken
    private Transform m_Player = null;
    EnemyHealth enemyHealth;
    playerHealth playerHealth;
    
    public int TypeAttack; // 0 = Melee, 1 = Range
    
    private void Awake() 
    {
        m_SpriteAnimator = transform.Find("Sprite").GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        playerHealth = GetComponent<playerHealth>();
        shurikken.SetActive(false);
    }

    private void Update() 
    {
        float distance = GetPlayerDistance();
        if (distance > 0f)
        {
            AttackorChase(distance);
        }
        else
        {
            m_State = EnemyState.Idle;
        }

        switch(m_State)
        {
            case EnemyState.Idle:
                OnIdle();
                break;
            case EnemyState.Chasing:
                OnChase();
                break;
            case EnemyState.AttackingMelee:
                OnAttackMelee();
                break;
            case EnemyState.AttackingRange:
                OnAttackRange();
                break;
        }
    }

    private void OnIdle() {}

    private void OnChase()
    {
        if (m_Player == null)
        {
            return;
        }

        Vector3 dir = (m_Player.position - transform.position).normalized;
        transform.position += m_Speed * Time.deltaTime * dir;
    }

    private void OnAttackMelee()
    {
        Debug.Log("Ataque Melee");
        m_SpriteAnimator.SetTrigger("MeleeAttack");
        TypeAttack = 0;
    }

    private void OnTriggerEnter(Collider other)
    {   
        float daño;
        if (TypeAttack == 0)
        {
            daño = UnityEngine.Random.Range(0.04f, 0.08f);
        }
        else
        {
            daño = 0;
        }

        if (other.CompareTag("Player"))
        {
            playerHealth.health -= daño;
            Debug.Log("Vida Jugador: " + playerHealth.health);
        }
    }

   private void OnAttackRange()
{
    Debug.Log("Ataque Rango");
    m_SpriteAnimator.SetTrigger("RangeAttack");
    TypeAttack = 1;

    // Iniciar el lanzamiento de shurikens solo si el cooldown lo permite
    if (canThrowShurikken)
    {
        StartCoroutine(ThrowShurikkenRepeatedly());
    }
}

// Corutina para lanzar shurikken repetidamente
private IEnumerator ThrowShurikkenRepeatedly()
{
    canThrowShurikken = false; // Bloquea nuevos lanzamientos hasta que se complete el cooldown

    while (m_State == EnemyState.AttackingRange) // Mientras esté en el estado de ataque a rango
    {
        // Mueve y lanza el shuriken
        if (shurikken != null)
        {
            GameObject shurikkenInstance = Instantiate(shurikken, transform.position, Quaternion.identity);  // Instancia un nuevo shurikken
            shurikkenInstance.SetActive(true); // Asegúrate de activar el shuriken
            MoveShurikken(shurikkenInstance); // Mueve el shuriken
            StartCoroutine(DestroyShurikkenAfterTime(shurikkenInstance, 3f)); // Destruye después de 3 segundos (ajustable)
        }

        // Espera el tiempo de cooldown antes de lanzar nuevamente
        yield return new WaitForSeconds(shurikkenCooldown);
    }

    // Después de salir del estado, se habilita el lanzamiento nuevamente
    canThrowShurikken = true;
}

// Corutina para destruir el shurikken después de un tiempo
private IEnumerator DestroyShurikkenAfterTime(GameObject shurikken, float timeToDestroy)
{
    yield return new WaitForSeconds(timeToDestroy); // Espera el tiempo determinado
    Destroy(shurikken); // Destruye el shurikken al final de su tiempo de vida
}

// Método para mover el shurikken
private void MoveShurikken(GameObject shurikkenInstance)
{
    // Obtener el Rigidbody2D del shuriken
    Rigidbody2D shurikkenIns = shurikkenInstance.GetComponent<Rigidbody2D>();

    // Desactivar la gravedad para que el shuriken no caiga
    shurikkenIns.gravityScale = 0f;

    // Determinar la dirección basada en la posición del jugador
    Vector2 direction = m_Player.position.x < transform.position.x ? Vector2.left : Vector2.right;

    // Ajustar la fuerza del shuriken
    float launchForce = 20f;

    // Limpiar la velocidad actual antes de aplicar la nueva fuerza
    shurikkenIns.velocity = Vector2.zero;

    // Aplicar la fuerza instantáneamente en la dirección correcta
    shurikkenIns.AddForce(direction * launchForce, ForceMode2D.Impulse);
}
    private void AttackorChase(float distance)
    {
        if (distance < m_AttackDistanceRange)
        {   
            if (distance < m_AttackDistanceMelee)
            {
                m_State = EnemyState.AttackingMelee;
            }
            else
            {
                m_State = EnemyState.AttackingRange;
            }
        }
        else
        {
            m_State = EnemyState.Chasing;
        }
    }

    private float GetPlayerDistance()
    {
        // Lanzas el Raycast
        var hit = Physics2D.Raycast(
            m_RaycastGenerator.position,
            Vector2.left,
            m_RaycastDistance,
            LayerMask.GetMask("Hitbox")
        );
        if (hit.collider != null)
        {
            // Hay una colision con el jugador
            m_Player = hit.collider.transform;
            Vector3 playerPos = m_Player.position;
            return Vector3.Distance(playerPos, transform.position);
        }

        hit = Physics2D.Raycast(
            m_RaycastGenerator.position,
            Vector2.right,
            m_RaycastDistance,
            LayerMask.GetMask("Hitbox")
        );
        if (hit.collider != null)
        {
            m_Player = hit.collider.transform;
            Vector3 playerPos = m_Player.position;
            return Vector3.Distance(playerPos, transform.position);
        }

        m_Player = null;
        return -1;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(
            m_RaycastGenerator.position,
            Vector2.left * m_RaycastDistance
        );
        Gizmos.DrawRay(
            m_RaycastGenerator.position,
            Vector2.right * m_RaycastDistance
        );
    }
}
