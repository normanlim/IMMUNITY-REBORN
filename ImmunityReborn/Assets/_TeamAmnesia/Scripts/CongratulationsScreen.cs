using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CongratulationsScreen : MonoBehaviour
{
    public TextMeshProUGUI timeScoreText;
    public Image easterEgg;
    public PlayerStateMachine playerState;

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
