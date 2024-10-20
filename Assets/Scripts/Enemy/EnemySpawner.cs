using System.Collections;
using UnityEngine;

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
    private PolygonCollider2D originalBounds;

    [SerializeField]
    public playerHealth pHealth;

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

    // Corrutina para generar enemigos en intervalos
    private IEnumerator SpawnEnemies()
    {
        while (spawnerActive && enemiesSpawned < maxEnemies)
        {
            SpawnEnemyInZone();
            enemiesSpawned++;
            yield return new WaitForSeconds(spawnInterval);
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
        // float yPosition = leftCollider.bounds.center.y;
        // Generar un valor aleatorio para la coordenada Y entre 0 y -5
        float randomY = Random.Range(0f, -5f);

        Vector2 spawnPosition = new Vector2(randomX, randomY);
        var newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        EnemyDamage enemyDamage = newEnemy.GetComponent<EnemyDamage>();
        if (enemyDamage != null)
        {
            enemyDamage.pHealth = pHealth;
        }

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

            // Desactivar el renderer del collider izquierdo
            var leftRenderer = leftCollider.GetComponent<Renderer>();
            if (leftRenderer != null)
            {
                leftRenderer.enabled = false;
            }
        }
        if (rightCollider != null)
        {
            rightCollider.enabled = false;

            // Desactivar el renderer del collider derecho
            var rightRenderer = rightCollider.GetComponent<Renderer>();
            if (rightRenderer != null)
            {
                rightRenderer.enabled = false;
            }
        }
    }
}
