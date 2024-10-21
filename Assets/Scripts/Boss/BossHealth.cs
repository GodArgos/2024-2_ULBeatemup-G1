using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public static event Action<Event> OnEnemyKilled;
    [SerializeField] float health, maxHealth = 100f;

    [SerializeField] private GameObject canvasHealth;
    private FloatingHealthBar healthBar;
    [SerializeField] CutSceneBoss cutSceneBoss;

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

    public void ActivateHealthBar()
    {
        if (!canvasHealth.activeInHierarchy)
        {
            canvasHealth.SetActive(true);
            healthBar = canvasHealth.transform.GetChild(0).GetComponent<FloatingHealthBar>();
            health = maxHealth;
            healthBar.UpdateHealthBar(health, maxHealth);
        }
    }
}
