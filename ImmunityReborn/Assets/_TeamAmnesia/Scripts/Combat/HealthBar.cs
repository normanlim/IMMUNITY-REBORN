using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBarSlider;
    [SerializeField]
    private Health health;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        healthBarSlider.value = 1.0f;
    }

    private void OnEnable()
    {
        health.OnTakeDamage += UpdateHealthBar;
        health.OnHeal += UpdateHealthBar;
        health.OnDie += HideHealthBar;
    }

    private void OnDisable()
    {
        health.OnTakeDamage -= UpdateHealthBar;
        health.OnHeal -= UpdateHealthBar;
        health.OnDie -= HideHealthBar;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }

    private void UpdateHealthBar()
    {
        healthBarSlider.value = (float)health.CurrentHealth / health.MaxHealth;
    }

    private void HideHealthBar()
    {
        healthBarSlider.gameObject.SetActive(false);
    }
}
