using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public static event Action<Event> OnEnemyKilled;
    [SerializeField] float health, maxHealth = 100f;
  
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] CutSceneBoss cutSceneBoss;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health,maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log("NEW HEALTH: "+health);
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        cutSceneBoss.StartCinematic();
        Destroy(gameObject);
    }
}
