using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField, Tooltip("Lista de puntos donde se spawnearán los enemigos")]
    Transform[] spawnPoints;
    [SerializeField, Tooltip("Prefab del enemigo a spawnear")]
    GameObject enemyPrefab;
    private List<GameObject> allEnemies = new List<GameObject>();

    [SerializeField, Tooltip("Retraso inicial antes de empezar a spawnear enemigos")]
    float initialSpawnDelay = 5f;
    [SerializeField, Tooltip("Reducción del retraso de spawn cada vez que se aumenta el número de enemigos")]
    float spawnDelayReduction = 1f;
    [SerializeField, Tooltip("El tiempo mínimo de retraso de spawn")]
    float maxSpawnDelay = 2f;

    [SerializeField, Tooltip("Cantidad de enemigos por oleada")]
    int enemiesPerWave = 3;
    private int maxEnemies = 30; // Cantidad máxima de enemigos en el nivel

    [SerializeField, Tooltip("Aumento de la salud del enemigo por cada nueva oleada")]
    float enemyHealthIncreasePerWave = 0.2f;
    [SerializeField, Tooltip("Aumento de la velocidad del enemigo por cada nueva oleada")]
    float enemySpeedIncreasePerWave = 0.5f;
    [SerializeField, Tooltip("Disminucion del cooldown del disparo por cada nueva oleada")]
    float enemyCooldownDecreasePerWave = 0.2f;
    [SerializeField, Tooltip("Disminucion del cooldown del disparo por cada nueva oleada")]
    float enemyPatrolDistanceIncreasePerWave = 3f;

    public int currentEnemies = 0; // Cantidad de enemigos vivos actualmente
    public int currentWave = -1; //Numero de oleada actual
    private float currentSpawnDelay; // Retraso actual entre spawns

    private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        // Iniciar con un retraso inicial antes de empezar a spawnear
        currentSpawnDelay = initialSpawnDelay;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies() {
        while (currentEnemies < maxEnemies) {
            //Se cuenta una nueva oleada
            currentWave += 1;
            if (currentWave > 3) {
                enemiesPerWave += 2;
            }
            Debug.Log($"Wave: {currentWave}");
            // Esperar el tiempo de retraso actual
            yield return new WaitForSeconds(currentSpawnDelay);

            // Spawnear una oleada de enemigos
            for (int i = 0; i < enemiesPerWave; i++) {
                SpawnEnemy();
            }

            // Incrementar la cantidad de enemigos vivos y reducir el tiempo de spawn
            currentEnemies += enemiesPerWave;
            allEnemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
            IncreaseParameters();
            currentSpawnDelay = Mathf.Max(currentSpawnDelay - spawnDelayReduction, maxSpawnDelay);
        }
    }

    private void SpawnEnemy() {
        if (player != null) {
            PlayerScript playerScript = player.GetComponent<PlayerScript>();

            // Elegir un punto de spawn aleatorio
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Instanciar el enemigo en el punto de spawn
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            // Instanciar el puntero para el nuevo enemigo
            playerScript.SpawnArrowPointer(newEnemy.transform);
        }
    }

    private void IncreaseParameters() {
        foreach(GameObject oneEnemy in allEnemies) {
            Enemy1Script enemyScript = oneEnemy.GetComponent<Enemy1Script>();
            enemyScript.SetMaxHealth(enemyScript.GetMaxHealth() + ((currentWave * 2) * enemyHealthIncreasePerWave));
            enemyScript.SetSpeed(enemyScript.GetSpeed() + ((currentWave * 2) * enemySpeedIncreasePerWave));
            enemyScript.SetTimeBtwShoots(enemyScript.GetTimeBtwShoots() - ((currentWave * 2) * enemyCooldownDecreasePerWave));
            enemyScript.SetPatrolDistance(enemyScript.GetPatrolDistance() + ((currentWave * 2) * enemyPatrolDistanceIncreasePerWave));
            enemyScript.OnDeath += OnEnemyDeath;
        }
    }

    private void OnEnemyDeath() {
        // Reducir la cantidad de enemigos vivos
        currentEnemies--;
    }

}