using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour, IHitbox
{
    [SerializeField]
    private Animator m_PlayerAnimator;
    [SerializeField] private playerHealth playerHealth;
    private float damageAmount;

    public void Hit(float optionalDamage)
    {
        playerHealth.TakeDamage(damageAmount != 0 ? damageAmount : optionalDamage);
        m_PlayerAnimator.SetTrigger("Damage");
    }
}
