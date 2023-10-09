using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarOverlayTMP : MonoBehaviour
{
    public Slider healthSlider; // Reference to your Slider component
    public TextMeshProUGUI healthText; // Reference to your TMP Text component

    private void Update()
    {
        // Update the text content with the health value
        healthText.text = healthSlider.value.ToString("0") + "/ 100";
    }
}
