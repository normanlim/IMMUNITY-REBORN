using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    public TextMeshProUGUI timeScoreText;
    public Image easterEgg;
    public PlayerStateMachine playerState;
    public GameObject softcoreText;
    public GameObject hardcoreText;

    private bool isEasterEggVisible = false;

    public void Setup()
    {
        gameObject.SetActive( true );

        float durationAlive = playerState.GetTimePlayerAlive();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        TimeSpan timeSpanAlive = TimeSpan.FromSeconds( durationAlive );
        string formattedTimeAlive = timeSpanAlive.ToString( @"hh\:mm\:ss" );

        timeScoreText.text = $"Elapsed Time: {formattedTimeAlive}";

        // Show heroic victory only when it's hardcore mode and impossible difficulty.
        if (PlayerPrefs.GetInt("Softcore", 0) == 1 && PlayerPrefs.GetInt("SelectedDifficulty") == 2)
        {
            softcoreText.SetActive(false);
            hardcoreText.SetActive(true);
        } else
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
