using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Script : MonoBehaviour {

    public delegate void OnDeathEventHandler();
    public event OnDeathEventHandler OnDeath;

    [Header("   Enemy Atributes")]
    [SerializeField] int health;
    [SerializeField] int maxHealth;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float startTimeBtwShoots;//cooldown para los disparos

    [Header("   Awareness Distances")]
    [SerializeField, Tooltip("distancia para seguir al jugador")] float stoppingDistance;
    [SerializeField, Tooltip("distancia para retirarse")] float retreatDistance;
    [SerializeField, Tooltip("distancia muy lejos para quedarse patrullando")] float patrolDistance;
    [SerializeField, Tooltip("cooldown para moverse")] float startWaitTime;

    [Header("   External References")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform[] moveSpots;
    [SerializeField] private Transform gunOffset; //distancia en la que se instanciara la bala
    [SerializeField] private GameObject prefab;
    [SerializeField] private AudioClip damageSound;

    private bool attacking = false;
    private float waitTime;//auxiliar en el cooldown para los disparos
    private float timeBtwShoots; //auxiliar en el cooldown para los disparos
    private Transform player; //posicion del jugador
    private Rigidbody2D rb;
    private int randomSpot;

    private void Start() {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        waitTime = startWaitTime;

        randomSpot = Random.Range(0, moveSpots.Length);
    }


    private void FixedUpdate() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Respawn").transform;
        }

        RotateLook();
        IsAttaking();
        PatrolMode();
        AttackingMode();
    }


    public void SetMaxHealth(float maxHealth) {
        this.maxHealth = (int)maxHealth;
    }

    public float GetMaxHealth() {
        return this.maxHealth;
    }

    public void SetSpeed(float speed) {
        this.speed = speed;
    }

    public float GetSpeed() {
        return this.speed;
    }

    public void SetTimeBtwShoots(float btwShoots) {
        if (btwShoots > 0.3f) {
            this.startTimeBtwShoots = btwShoots;
        } else {
            this.startTimeBtwShoots = 0.3f;
        }
    }

    public float GetTimeBtwShoots() {
        return startTimeBtwShoots;
    }

    public void SetPatrolDistance(float patrolDistance) {
        this.patrolDistance = patrolDistance;
    }

    public float GetPatrolDistance() {
        return patrolDistance;
    }


    public void Die() {
        Destroy(prefab);
        if (OnDeath != null) {
            OnDeath();
        }
    }


    private void Shoot() {
        if (timeBtwShoots <= 0) {

            Instantiate(bullet, gunOffset.position, transform.rotation);
            timeBtwShoots = startTimeBtwShoots;
        } else {
            timeBtwShoots -= Time.deltaTime;
        }
    }


    public void TakeDamage(int damage) {
        health -= damage;
        AudioControler.Instance.PlaySound(damageSound);
        if (health <= 0) {
            Die();
            return;
        }
    }


    private void RotateLook() {
        //obtenemos la posicion del personaje
        Vector2 targetDirection = (player.position - transform.position).normalized;
        Quaternion rotation;

        //comprobamos si esta en modo de ataque o patrullaje
        if (attacking) {
            //definimos que la parte de arriba (transform.forward) siga al personaje
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, targetDirection);
            //guardamos en la variable la rotacion actual, a donde vamos a rotar, y la velocidad de rotacion
            rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            //movemos el rigidbody y lo rotamos hacia donde establecimos
            rb.SetRotation(rotation);

        } else {
            //si esta patrullando, guardamos como objetivo que mire hacia arriba
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, Vector2.up);
            //del mismo modo rotamos desde donde estamos, a donde queremos mirar con la velocidad
            rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            //rotamos el rigidbody
            rb.SetRotation(rotation);
        }
    }


    private void IsAttaking() {
        //Si el jugador esta mas cerca que la variable de patrullaje
        if (Vector2.Distance(transform.position, player.position) < patrolDistance) {
            //entra en modo ataque
            attacking = true;
        } else {
            //entra en modo patrullaje
            attacking = false;
        }
    }


    private void AttackingMode() {
        if (attacking) {
            //Si el jugador no esta demasiado lejos como para patrullar, entrara en modo ataque
            if (Vector2.Distance(transform.position, player.position) < patrolDistance) {

                //si el jugador esta muy lejos
                if (Vector2.Distance(transform.position, player.position) > stoppingDistance) {
                    //el enemigo se acercara hacia el jugador
                    transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

                    //si esta lo suficientemente cerca
                } else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance) {

                    //que el enemigo se quede quieto
                    transform.position = this.transform.position;

                    //Si el jugador se acerca mucho
                } else if (Vector2.Distance(transform.position, player.position) < retreatDistance) {
                    //el enemigo se alejara del jugador
                    transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
                }
                Shoot();
            }
        }
    }


    private void PatrolMode() {
        if (!attacking) {


            transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.1f) {
                if (waitTime <= 0) {
                    randomSpot = Random.Range(0, moveSpots.Length);
                    waitTime = startWaitTime;
                } else {
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }
}