using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LogoColorChange : MonoBehaviour
{
    public Image image;
    public float colorChangeDuration = 0.5f;
    public float delayBetweenChanges = 3.0f;

    private Color[] targetColors;
    private int currentIndex = 0;

    private void Start()
    {
        // Red Green Blue - trasparent 50%
        targetColors = new Color[] { new Color(1.0f, 0.0f, 0.0f, 0.32f), new Color(0.0f, 0.0f, 1.0f, 0.32f), new Color(0.0f, 1.0f, 0.0f, .32f) };
        StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        while (true)
        {
            int nextIndex = (currentIndex + 1) % targetColors.Length;
            Color startColor = image.color;
            Color endColor = targetColors[nextIndex];
            float elapsedTime = 0;

            while (elapsedTime < colorChangeDuration)
            {
                image.color = Color.Lerp(startColor, endColor, elapsedTime / colorChangeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            image.color = endColor;
            currentIndex = nextIndex;
            yield return new WaitForSeconds(delayBetweenChanges);
        }
    }
}