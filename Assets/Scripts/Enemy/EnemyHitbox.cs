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



    public void Hit(float optionalDamage )
    {
        var light = m_EnemyAnimator.
            gameObject.transform.Find("Light");;
        light.gameObject.SetActive(true);
        enemyHealth.TakeDamage(damageAmount != 0 ? damageAmount : optionalDamage);
        m_EnemyAnimator.SetTrigger("ReceiveAttack");
    }
}
