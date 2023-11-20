using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthGauge : MonoBehaviour
{

    public int current;
    public int maximum;
    public Slider healthGaugeSlider;
    public PlayerStateMachine stateMachine;

    private void Start()
    {
        maximum = stateMachine.Health.MaxHealth;
        current = stateMachine.Health.MaxHealth;
    }

    private void OnEnable()
    {
        stateMachine.Health.OnTakeDamage += UpdateHealthGaugeSlider;
        stateMachine.Health.OnHeal += UpdateHealthGaugeSlider;
    }

    private void OnDisable()
    {
        stateMachine.Health.OnTakeDamage -= UpdateHealthGaugeSlider;
        stateMachine.Health.OnHeal -= UpdateHealthGaugeSlider;
    }

    private void UpdateHealthGaugeSlider()
    {
        healthGaugeSlider.value = stateMachine.Health.CurrentHealth;
    }

}
