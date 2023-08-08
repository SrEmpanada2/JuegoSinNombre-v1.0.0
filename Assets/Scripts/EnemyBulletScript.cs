using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public float bulletSpeed;
    [SerializeField] private int damage;
    [SerializeField] private float lifetime;
    Rigidbody2D rb;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        Invoke("DestroyBullet", lifetime);
    }

    private void FixedUpdate() {
        rb.velocity = transform.up * bulletSpeed;
    }

    private void DestroyBullet() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth player)) {
            player.TakeDamage(damage);
            Destroy(gameObject);
        } else if (collision.gameObject.TryGetComponent<Enemy1Script>(out Enemy1Script enemy)) {
            return;
        } else if (collision.gameObject.TryGetComponent<BulletScript>(out BulletScript bullet)) {
            return;
        }
        Destroy(gameObject);
    }

    
}
