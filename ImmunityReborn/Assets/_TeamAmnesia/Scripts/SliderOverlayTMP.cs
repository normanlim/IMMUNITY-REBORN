using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderOverlayTMP : MonoBehaviour
{
    public Slider slider; // Reference to your Slider component
    public TextMeshProUGUI sliderText; // Reference to your TMP Text component

    private void Update()
    {
        // Update the text content with the health value
        sliderText.text = slider.value.ToString("0") + "/ 100";
    }
}
