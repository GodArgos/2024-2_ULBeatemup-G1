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
    private EnemyState m_State = EnemyState.Idle;
    private Animator m_SpriteAnimator;
  
    private bool m_IsTalking = false;

    private Transform m_Player = null;

    private void Awake() 
    {
        m_SpriteAnimator = transform.Find("Sprite").GetComponent<Animator>();
        
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
        
            var hit = Physics2D.Raycast(
            m_RaycastGenerator.position,
            Vector2.left,
            m_RaycastDistance,
            LayerMask.GetMask("Hitbox"));
            if (hit.collider != null)
            {
            // Hay una colision con player
                playerHealth.Instance.health -= UnityEngine.Random.Range(0.04f, 0.08f); 
                Debug.Log("Vida Jugador: " + playerHealth.Instance.health);
            }
                   
    }

     private void OnAttackRange()
    { 
        Debug.Log("Ataque Range");
        m_SpriteAnimator.SetTrigger("RangeAttack");
       
        EnemyHealth.Instance.health -= UnityEngine.Random.Range(0.001f, 0.03f); 
        Debug.Log("Vida Enemigo: " + EnemyHealth.Instance.health);
    }

    private void AttackorChase(float distance)
    {
        if (distance < m_AttackDistanceRange)
        {   
                m_State = EnemyState.AttackingRange;
            
        }else if(distance < m_AttackDistanceMelee){
            m_State = EnemyState.AttackingMelee;
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
