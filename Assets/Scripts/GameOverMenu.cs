using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;
    private PlayerHealth playerHealth;

    private void Start() {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerHealth.PlayerDeath += ActivateMenu;
    }

    private void ActivateMenu(object sender, EventArgs e) {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu(string menu) {
        SceneManager.LoadScene(menu);
    }
}
