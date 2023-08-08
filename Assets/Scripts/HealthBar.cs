using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;

    private void Start() {
        slider = GetComponent<Slider>();
    }

    public void ChangeMaxHealth(float maxHealth) {
        slider.maxValue = maxHealth;
    }

    public void ChangeActualHealth(float health) {
        slider.value = health;
    }

    public void InitializeHealth(float health) {
        ChangeMaxHealth(health);
        ChangeActualHealth(health);
    }
}
