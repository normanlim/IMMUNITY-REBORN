using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Text difficultyText;
    public Text softcoreText;
    public Text godmodeText;
    void Start()
    {
        if (PlayerPrefs.GetInt("SelectedDifficulty") == 0)
            difficultyText.text = "Difficulty: Normal";
        else if (PlayerPrefs.GetInt("SelectedDifficulty") == 1)
            difficultyText.text = "Difficulty: Hard";
        else if (PlayerPrefs.GetInt("SelectedDifficulty") == 2)
            difficultyText.text = "Difficulty: Impossible";
        if (PlayerPrefs.GetInt("Softcore", 0) == 0)
            softcoreText.text = "Softcore: Off";
        else
            softcoreText.text = "Softcore: On";
        if (PlayerPrefs.GetInt("GodMode", 0) == 0)
            godmodeText.text = "Godmode: Off";
        else
            godmodeText.text = "Godmode: On";
    }
}
