using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickRotation : MonoBehaviour
{
    public FloatingJoystick joystick;
    [SerializeField] private float deadzone = 0.1f; // Valor de la zona muerta

    private Vector2 move;
    private bool isInDeadzone = false;

    private void Update() {
        move.x = joystick.Horizontal * -1f;
        move.y = joystick.Vertical;


        if (move.magnitude < deadzone) {
            // Si está en la zona muerta, asegurarse de que no haya rotación y activar el flag
            isInDeadzone = true;

        } else {

            float hAxis = move.x;
            float vAxis = move.y;
            float zAxis = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0f, 0f, zAxis);

            if (isInDeadzone) {
                Vibrator.Vibrate(100);
                isInDeadzone = false;
            }
        }
    }
}
