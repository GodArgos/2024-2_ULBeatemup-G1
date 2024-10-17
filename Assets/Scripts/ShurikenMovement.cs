using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenMovement : MonoBehaviour
{
    public float speed = 10f; // Velocidad del shuriken
    public float lifetime = 4f; // Tiempo antes de que el shuriken se destruya
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        // Mover el shuriken constantemente en la dirección dada
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // Reducir el tiempo de vida del shuriken
        lifetime -= Time.deltaTime;

        // Destruir el shuriken después de que pase el tiempo de vida
        if (lifetime <= 0f)
        {
            Destroy(gameObject); // Destruye el objeto (shuriken)
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si el shuriken golpea al jugador (o un enemigo)
        if (collision.CompareTag("Player"))
        {
            // Aplica el daño al jugador aquí (puedes acceder a la vida del jugador)
            Debug.Log("Shuriken golpeó al jugador.");
            Destroy(gameObject); // Destruye el shuriken al golpear
        }
    }
}
