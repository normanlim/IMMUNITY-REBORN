using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthGauge : MonoBehaviour
{
    public Slider healthGaugeSlider; // extra

    public PlayerStateMachine stateMachine;
    public Image healthBall;

    private void Start()
    {
        healthBall.fillAmount = stateMachine.Health.CurrentHealth / stateMachine.Health.MaxHealth;
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
        healthBall.fillAmount = (float)stateMachine.Health.CurrentHealth / stateMachine.Health.MaxHealth;

        healthGaugeSlider.value = stateMachine.Health.CurrentHealth;
    }

}
