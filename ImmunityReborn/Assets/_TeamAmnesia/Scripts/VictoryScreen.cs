using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    public TextMeshProUGUI timeScoreText;
    public TextMeshProUGUI victoryHeaderText;
    public Image easterEgg;
    public PlayerStateMachine playerState;
    public GameObject softcoreText;
    public GameObject hardcoreText;
    public Color softcoreColor;
    public Color hardcoreColor;

    private bool isEasterEggVisible = false;

    public void Setup()
    {
        gameObject.SetActive(true);

        float durationAlive = playerState.GetTimePlayerAlive();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Set victory header text based on game 
        // Add prefix based on difficulty (no prefix for normal mode)
        string difficultyPrefix = "";
        if (PlayerPrefs.GetInt("SelectedDifficulty") == 1)
            difficultyPrefix += "HARD ";
        else if (PlayerPrefs.GetInt("SelectedDifficulty") == 2)
            difficultyPrefix += "IMPOSSIBLE ";

        // Set text content first
        victoryHeaderText.text = difficultyPrefix + "VICTORY!";

        // Change text color and add prefix based on softcore or hardcore
        if (PlayerPrefs.GetInt("Softcore", 0) == 1)
        {
            victoryHeaderText.color = softcoreColor;
        }
        else
        {
            // Add prefix after setting the text content
            victoryHeaderText.text = "HEROIC " + victoryHeaderText.text;
            victoryHeaderText.color = hardcoreColor;
        }

        // Set time score
        TimeSpan timeSpanAlive = TimeSpan.FromSeconds(durationAlive);
        string formattedTimeAlive = timeSpanAlive.ToString(@"hh\:mm\:ss");

        timeScoreText.text = $"Elapsed Time: {formattedTimeAlive}";

        // Show heroic victory only when it's hardcore mode and impossible difficulty.
        if (PlayerPrefs.GetInt("Softcore", 0) == 0 && PlayerPrefs.GetInt("SelectedDifficulty") == 2)
        {
            softcoreText.SetActive(false);
            hardcoreText.SetActive(true);
        }
        else
        {
            softcoreText.SetActive(true);
            hardcoreText.SetActive(false);
        }
    }

    public void ToggleEasterEggImageVisibility()
    {
        isEasterEggVisible = !isEasterEggVisible;
        easterEgg.gameObject.SetActive( isEasterEggVisible );
    }

    public void RestartGameButton()
    {
        isEasterEggVisible = false; 

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene( currentSceneName );
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene( "MenuScreen" );
    }
}
