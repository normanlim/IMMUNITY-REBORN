using UnityEngine;

public class Menu : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("ExitGame button clicked");
        Application.Quit();
    }
}
