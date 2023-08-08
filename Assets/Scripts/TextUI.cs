using UnityEngine;
using TMPro;

public class TextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveCounter;
    [SerializeField] private TextMeshProUGUI enemiesRemain;
    private GameObject spawner;

    private void Start() {
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner");
    }

    private void Update() {
        EnemySpawner enemySpawner = spawner.GetComponent<EnemySpawner>();
        string wave = "Oleada: " + (enemySpawner.currentWave - 1).ToString();
        string enemiesCounter = "Enemigos Restantes: " + enemySpawner.currentEnemies.ToString();
        waveCounter.text = wave;
        enemiesRemain.text = enemiesCounter;
    }

}
