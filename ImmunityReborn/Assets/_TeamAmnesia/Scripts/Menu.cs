using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Dropdown DifficultyDropdown;
    [SerializeField] Toggle SoftcoreToggle;
    [SerializeField] Toggle GodModeToggle;

    private void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {
        // Set God Mode Toggle based on PlayerPrefs
        bool godModeEnabled = PlayerPrefs.GetInt("GodMode", 0) == 1;
        GodModeToggle.isOn = godModeEnabled;

        // Set God Mode Toggle based on PlayerPrefs
        bool softcoreEnabled = PlayerPrefs.GetInt("Softcore", 0) == 1;
        SoftcoreToggle.isOn = softcoreEnabled;

        // Set Difficulty Dropdown based on PlayerPrefs
        int selectedDifficulty = PlayerPrefs.GetInt("SelectedDifficulty", 0);
        DifficultyDropdown.value = selectedDifficulty;

        // Reset last known level in softcore mode
        PlayerPrefs.SetInt("SCCurrentLevel", 0);
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame button clicked");
        Application.Quit();
    }
}
