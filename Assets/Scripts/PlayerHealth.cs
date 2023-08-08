using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;

    public event EventHandler PlayerDeath;

    private void Start() {
        health = maxHealth;
        healthBar.InitializeHealth(health);
    }

    public void TakeDamage(int damage) {
        health -= damage;
        healthBar.ChangeActualHealth(health);
        if (health <= 0) {
            PlayerDeath?.Invoke(this, EventArgs.Empty);
            AudioControler.Instance.PlaySound(deathSound);
            Destroy(gameObject);
            return;
        }
        AudioControler.Instance.PlaySound(damageSound);
    }

}
