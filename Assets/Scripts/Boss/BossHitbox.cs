using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitbox : MonoBehaviour, IHitbox
{
    [SerializeField]
    private BossMovement m_bossMovement;
    [SerializeField]
    private Animator m_BossAnimator;

    public void Hit()
    {
        if (!m_bossMovement.IsCharging)
        {
            m_BossAnimator.SetTrigger("ReceiveAttack");
        }
    }
}
