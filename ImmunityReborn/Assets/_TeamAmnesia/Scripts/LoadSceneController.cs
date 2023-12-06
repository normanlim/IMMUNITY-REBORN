using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneController : MonoBehaviour
{
    public GameObject LoadingBar;
    public Slider sliderComp;

    public GameObject MainMenuStack;


    public void LoadScene( int sceneId )
    {
        StartCoroutine( LoadSceneAsync( sceneId ) );
    }

    IEnumerator LoadSceneAsync( int sceneId )
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync( sceneId );
        operation.allowSceneActivation = false; // Prevent automatic scene activation at 90%

        LoadingBar.SetActive( true );
        MainMenuStack.SetActive( false );

        while ( operation.progress < 0.9f )
        {
            // Update the slider's value to match the loading progress
            sliderComp.value = operation.progress;
            yield return null;
        }

        // Manually handle the last 10%
        while ( sliderComp.value < 1f )
        {
            sliderComp.value += Time.deltaTime;
            if ( sliderComp.value >= 1f )
            {
                // Allow the scene to activate
                operation.allowSceneActivation = true;
            }
            yield return null;
        }

    }
}
