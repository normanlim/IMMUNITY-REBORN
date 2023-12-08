using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenu;

    public static bool isPaused;

    private GameObject freelookCamera;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        freelookCamera = GameObject.Find( "Main Camera" );
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Escape) )
        {
            if( !isPaused )
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive( true );
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Deal with camera when pause, make sure it does not move out of bounds when paused while catching up to players movement
        var cinemachineBrain = freelookCamera.GetComponent<CinemachineBrain>();
        if ( cinemachineBrain != null )
        {
            cinemachineBrain.enabled = false;
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive( false );
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Deal with camera when pause, make sure it does not move out of bounds when paused while catching up to players movement
        var cinemachineBrain = freelookCamera.GetComponent<CinemachineBrain>();
        if ( cinemachineBrain != null )
        {
            cinemachineBrain.enabled = true;
        }
    }

    public void ExitGameButton()
    {
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Application.Quit();
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene("MenuScreen");
    }
}
