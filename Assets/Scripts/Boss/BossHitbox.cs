using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitbox : MonoBehaviour, IHitbox
{
    [SerializeField]
    private BossMovement m_bossMovement;
    [SerializeField]
    private Animator m_BossAnimator;
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private float damageAmount;

    public void Hit(float optionalDamage)
    {
        if (!m_bossMovement.IsCharging)
        {
            bossHealth.TakeDamage(damageAmount != 0 ? damageAmount : optionalDamage);
            m_BossAnimator.SetTrigger("ReceiveAttack");
        }
    }
}
