using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthConsumable : MonoBehaviour
{
    [field: SerializeField]
    public Health TargetHealth { get; private set; }

    [field: SerializeField]
    public int HealAmount { get; private set; } = 20;

    [field: SerializeField]
    public int MaxItemCount { get; private set; } = 3;

    [field: SerializeField]
    public int CurrentItemCount { get; private set; }

    [field: SerializeField]
    public Image ItemCountBall { get; private set; }

    [field: SerializeField]
    public TMP_Text ItemCountText { get; private set; }

    [field: SerializeField]
    public GameObject HealEffect { get; private set; }

    private void Start()
    {
        CurrentItemCount = MaxItemCount;
        ItemCountBall.fillAmount = (float)CurrentItemCount / MaxItemCount;
        ItemCountText.text = CurrentItemCount.ToString();
    }

    public void Use()
    {
        if (CurrentItemCount > 0)
        {
            TargetHealth.Heal(HealAmount);
            Instantiate(HealEffect, transform);
            CurrentItemCount--;
            ItemCountBall.fillAmount = (float)CurrentItemCount / MaxItemCount;
            ItemCountText.text = CurrentItemCount.ToString();
        }
    }

    public void AddItemCount(int count)
    {
        CurrentItemCount = CurrentItemCount + count > MaxItemCount ? MaxItemCount : CurrentItemCount + count;
        ItemCountBall.fillAmount = (float)CurrentItemCount / MaxItemCount;
        ItemCountText.text = CurrentItemCount.ToString();
    }

    public void SetHealAmount(int healAmount)
    {
        HealAmount = healAmount;
    }
}
