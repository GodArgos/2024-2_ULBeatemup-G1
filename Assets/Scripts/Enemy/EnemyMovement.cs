using System;
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
    private EnemyState  m_State= EnemyState.Idle;
    private Animator m_SpriteAnimator;
    public GameObject shurikken;
    private bool m_IsTalking = false;

    private Transform m_Player = null;
    EnemyHealth enemyHealth;
    playerHealth playerHealth;
    
    public int TypeAttack; //0 = Melee and 1 = Range
    
    private void Awake() 
    {
        m_SpriteAnimator = transform.Find("Sprite").GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        playerHealth = GetComponent<playerHealth>();
        shurikken.SetActive(false);
        //m_RaycastGenerator = transform.Find("RaycastGenerator");
    }

    private void Update() 
    {
        float distance = GetPlayerDistance();
        if (distance > 0f)
        {
            AttackorChase(distance);
        }else{
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

    private void OnIdle()
    {}

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
        if(TypeAttack == 0){
            daño = UnityEngine.Random.Range(0.04f, 0.08f);
        }else{
            daño = 0;
        }

        if(other.CompareTag("Player"))
        {
            playerHealth.health -= daño;
            Debug.Log("Vida Jugador: " + playerHealth.health);
        }
    }

     private void OnAttackRange()
    { 
        Debug.Log("Ataque Range");
        m_SpriteAnimator.SetTrigger("RangeAttack");
        TypeAttack = 1;
        
         // Mover el shurikken
        if (shurikken != null)
        {
            shurikken.SetActive(true);
        // Esto moverá el shurikken 
           MoveShurikken(shurikken);
        }
        
    }

    // Método para mover el shurikken
    // Método para mover el shurikken
    private void MoveShurikken(GameObject shurikken)
{
    // Obtener el Rigidbody2D del shuriken
    Rigidbody2D shurikkenIns = shurikken.GetComponent<Rigidbody2D>();
    
    // Desactivar la gravedad para que el shuriken no caiga
    shurikkenIns.gravityScale = 0f;

    // Determinar la dirección según hacia dónde está mirando el enemigo (localScale.x)
    // Si la escala en X es positiva, el enemigo mira a la derecha, si es negativa, a la izquierda
    Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    // Ajustar la fuerza del shuriken
    float launchForce = 20f;  // Puedes ajustar este valor según la fuerza deseada

    // Limpiar la velocidad actual antes de aplicar la nueva fuerza
    shurikkenIns.velocity = Vector2.zero;

    // Aplicar la fuerza instantáneamente en la dirección correcta
    shurikkenIns.AddForce(direction * launchForce, ForceMode2D.Impulse);

    // Si deseas destruir el shuriken después de un tiempo (esto es opcional)
    //Destroy(shurikken, 2f);  // Destruir el shuriken después de 2 segundos (ajusta el tiempo si es necesario)
}




    private void AttackorChase(float distance)
    {
        if (distance < m_AttackDistanceRange)
        {   
            if(distance < m_AttackDistanceMelee){
            m_State = EnemyState.AttackingMelee;
            }else{
                m_State = EnemyState.AttackingRange;
            }
        }else
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
            // Hay una colision con player
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
            // Hay una colision con enemigo
            m_Player = hit.collider.transform;
            Vector3 playerPos = m_Player.position;
            return Vector3.Distance(playerPos, transform.position);
        }

        m_Player = null;
        return -1;
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
