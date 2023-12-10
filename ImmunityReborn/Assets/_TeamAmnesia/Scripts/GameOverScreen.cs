using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI timeScoreText;

    public void Setup( float durationAlive )
    {
        gameObject.SetActive( true );

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        TimeSpan timeSpanAlive = TimeSpan.FromSeconds( durationAlive );
        string formattedTimeAlive = timeSpanAlive.ToString( @"hh\:mm\:ss" );

        timeScoreText.text = $"Elapsed Time: {formattedTimeAlive}";
    }

    public void RestartGameButton()
    {
        // Save the current level index for softcore
        PlayerPrefs.SetInt("SCCurrentLevel", FindFirstObjectByType<LevelManager>().CurrentLevelIndex);
        // Get the name of the current scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Load the current scene again to reset it
        SceneManager.LoadScene( currentSceneName );
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene( "MenuScreen" );
    }
}
