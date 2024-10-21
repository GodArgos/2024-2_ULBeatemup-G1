using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // Importar SceneManager

public class playerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBar;

    void Start()
    {
        maxHealth = health;
    }

    void Update()
    {
        // Actualizar la barra de salud
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);

        // Verificar si la salud llega a 0 o menos
        if (health <= 0)
        {
            ResetLevel();  // Llamar a la función que reinicia el nivel
        }
    }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log("TROOK ASDAS");
        health -= damageAmount;
    }

    void ResetLevel()
    {
        // Reiniciar el nivel cargando la escena actual de nuevo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
