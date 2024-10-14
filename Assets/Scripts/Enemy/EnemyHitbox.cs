using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyHitbox : MonoBehaviour, IHitbox
{
    [SerializeField]
    private Animator m_EnemyAnimator;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private float damageAmount;



    public void Hit()
    {
        var light = m_EnemyAnimator.
            gameObject.transform.Find("Light");;
        light.gameObject.SetActive(true);
        enemyHealth.TakeDamage(damageAmount);
        m_EnemyAnimator.SetTrigger("ReceiveAttack");
    }
}
