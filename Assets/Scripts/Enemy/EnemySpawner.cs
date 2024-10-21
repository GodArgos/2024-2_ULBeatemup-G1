using System.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public Collider2D leftCollider; // Collider izquierdo de la zona
    public Collider2D rightCollider; // Collider derecho de la zona

    public float spawnInterval = 3f; // Tiempo entre cada spawn
    private bool spawnerActive = false; // Controla si el spawner está activo
    private int enemiesSpawned = 0; // Contador de enemigos generados
    private int enemiesDefeated = 0; //Contador de enemigos derrotados
    public int maxEnemies = 3; // Máximo número de enemigos a generar

    [SerializeField]
    private FloatingHealthBar firstEnemyHealthBar;
    
    [SerializeField]
    private PolygonCollider2D originalBounds;

    [SerializeField]
    public playerHealth pHealth;

    private GameObject currentEnemy; // Referencia al enemigo actual

    private void Start()
    {
        // Desactiva los colliders al inicio, ya que la zona no está activa aún
        leftCollider.enabled = false;
        rightCollider.enabled = false;
        // ActivateSpawner();
    }

    // Método para activar el spawner tras la cutscene
    public void ActivateSpawner()
    {
        // Activa los colliders de la zona
        leftCollider.enabled = true;
        rightCollider.enabled = true;

        // Activa el spawner
        spawnerActive = true;
        StartCoroutine(SpawnEnemies());
    }

    // Corrutina para generar enemigos cuando la salud del enemigo actual baja del 75%
    private IEnumerator SpawnEnemies()
    {
        // Comprobar si hay enemigos para spawnear y si el spawner está activo
        while (spawnerActive && enemiesSpawned < maxEnemies)
        {
            // Si es el primer enemigo, usamos la barra de vida proporcionada
            if (enemiesSpawned == 0)
            {
                yield return new WaitUntil(() => firstEnemyHealthBar != null && GetHealthPercentage(firstEnemyHealthBar) < 0.75f);
            }
            else
            {
                // Espera hasta que el enemigo actual tenga menos del 75% de vida
                if (currentEnemy != null)
                {
                    FloatingHealthBar enemyHealth = currentEnemy.transform.GetChild(2).GetChild(0).GetComponent<FloatingHealthBar>();
                    if (enemyHealth != null)
                    {
                        yield return new WaitUntil(() => GetHealthPercentage(enemyHealth) < 0.75f);
                    }
                }
            }

            // Spawn el siguiente enemigo
            SpawnEnemyInZone();
            enemiesSpawned++;
            //yield return new WaitForSeconds(spawnInterval);
        }

        // Desactiva el spawner una vez que se generaron los enemigos
        spawnerActive = false;
    }

    // Método para instanciar el enemigo en una posición aleatoria dentro de la zona
    private void SpawnEnemyInZone()
    {
        float minX = leftCollider.bounds.max.x;
        float maxX = rightCollider.bounds.min.x;
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(0f, -5f);

        Vector2 spawnPosition = new Vector2(randomX, randomY);
        currentEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    // Llamar a este método desde el script del enemigo cuando muera
    public void EnemyDefeated()
    {
        enemiesDefeated++;

        // Si todos los enemigos han sido derrotados
        if (enemiesDefeated >= maxEnemies + 1)
        {
            ResetAfterBattle();
        }
    }

    private void ResetAfterBattle()
    {
        // Cambiar los bounds de la cámara a los originales
        GameManager.Instance.ChangeBounds(originalBounds);

        // Desactivar los colliders de la zona y sus renderers
        if (leftCollider != null)
        {
            leftCollider.enabled = false;

            var leftRenderer = leftCollider.GetComponent<Renderer>();
            if (leftRenderer != null)
            {
                leftRenderer.enabled = false;
            }
        }

        if (rightCollider != null)
        {
            rightCollider.enabled = false;

            var rightRenderer = rightCollider.GetComponent<Renderer>();
            if (rightRenderer != null)
            {
                rightRenderer.enabled = false;
            }
        }
    }

    // Método para obtener el porcentaje de salud de la barra de vida
    private float GetHealthPercentage(FloatingHealthBar healthBar)
    {
        if (healthBar != null)
        {
            return healthBar.slider.value; // Devuelve el valor normalizado (entre 0 y 1)
        }
        return 1f; // Si no hay barra, devolver 1 (100% de salud)
    }
}
