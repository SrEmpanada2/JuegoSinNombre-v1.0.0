using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    //Atributos del jugador
    [Header("   Player Movement")]
    
    [SerializeField] float maxSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float acceleration = 10f;  // Aceleración del personaje
    [SerializeField] float deceleration = 5f;  // Deceleración del personaje


    [Header("   Bullet Atributes")]
    [SerializeField, Range(0f,1f)] private float delayShoot;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private Transform gunOffset; //distancia en la que se instanciara la bala

    [Header("")]
    [SerializeField] GameObject arrowPrefab;

    private Rigidbody2D rb;

    private float lastShootTime;
    private Vector3 initialOrientation;

    //Instanciamos las clases al iniciar el juego
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 1;

        initialOrientation = Input.acceleration;
    }

    void Update() {

        //al presionar la pantalla, detecta un delay y manda llamar la funcion shoot
        if (Input.touchCount > 0) {

            // Iteramos a través de todos los toques
            for (int i = 0; i < Input.touchCount; i++) {

                //guardamos el toque en una variable
                Touch touch = Input.GetTouch(i);

                //comprobamos si este toque esta del lado derecho de la pantalla
                if (touch.position.x > Screen.width / 2f) {

                    //Creamos un delay entre los toques para no tener muchas balas 
                    if (touch.phase == TouchPhase.Began) {

                        float timeSinceLastShoot = Time.time - lastShootTime;

                        if (timeSinceLastShoot >= delayShoot) {
                            Shoot();

                            lastShootTime = Time.time;
                        }
                    }
                }
            }
        }
    }


    //Physics
    private void FixedUpdate() {
        PlayerMovement();
        //MovimientoTeclado();
    }


    private void PlayerMovement() {
        // Obtener el valor de inclinación del acelerómetro
        Vector3 currentOrientation = Input.acceleration;

        // Calcular la dirección y velocidad del movimiento
        Vector3 movementDirection = currentOrientation - initialOrientation;
        float movementSpeed = Mathf.Clamp(movementDirection.magnitude * acceleration, 0f, maxSpeed);

        // Aplicar el movimiento al personaje
        rb.velocity = movementDirection.normalized * movementSpeed;

        // Decelerar si el dispositivo no está inclinado
        if (movementDirection.magnitude < 0.1f) {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }
    }


    //activa una bala y la guarda en una variable para establecerle la posicion de inicio
    private void Shoot() {
        Instantiate(bulletPrefab, gunOffset.position, transform.rotation);
    }


    public void SpawnArrowPointer(Transform target) {
        GameObject arrowPointer = Instantiate(arrowPrefab, transform);
        ArrowPoint arrowPointScript = arrowPointer.GetComponent<ArrowPoint>();
        arrowPointScript.SetTarget(target);
    }


    private void MovimientoTeclado() {
        float Horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");
        //Vector2 que detecta la entradas en x, y. Ademas de normalizar el vector
        Vector2 moveInput = new Vector2(Horizontal, Vertical).normalized;

        rb.MovePosition(rb.position + moveInput * maxSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.J)) {
            transform.Rotate(Vector3.forward * rotationSpeed);
        } else if (Input.GetKey(KeyCode.K)) {
            transform.Rotate(Vector3.back * rotationSpeed);
        }

        //al presionar space, detecta un delay y manda llamar la funcion shoot
        if (Input.GetKeyDown(KeyCode.Space)) {
            float timeSinceLastShoot = Time.time - lastShootTime;

            if (timeSinceLastShoot >= delayShoot) {
                Shoot();

                lastShootTime = Time.time;
            }
        }
    }
}