using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image bar;
    [SerializeField] private Color maxHealth;
    [SerializeField] private Color mediumHealth;
    [SerializeField] private Color lowHealth;
    private int currentlvl;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;

        // Calcula el porcentaje de vida actual
        float healthPercentage = currentValue / maxValue;

        // Cambia el color de la barra dependiendo del porcentaje de vida
        if (healthPercentage > 0.5f)
        {
            bar.color = maxHealth;  // Si la vida es mayor al 50%, verde
        }
        else if (healthPercentage > 0.25f)
        {
            bar.color = mediumHealth;  // Si la vida es mayor al 25% y menor o igual al 50%, amarillo
        }
        else
        {
            bar.color = lowHealth;  // Si la vida es menor o igual al 25%, rojo
        }

        Debug.Log("Color Actual: " + bar.color);
        Debug.Log("Valor del Slider: " + slider.value);
    }

    //public void Update() {
}
