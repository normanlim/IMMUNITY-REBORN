using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject MenuPlaySND;
    public void PlayGame()
    {
        Debug.Log("StartGame button clicked");
        PlaySFX.PlayThenDestroy(MenuPlaySND, transform);
        SceneManager.LoadScene("Betac");
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame button clicked");
        Application.Quit();
    }
}
