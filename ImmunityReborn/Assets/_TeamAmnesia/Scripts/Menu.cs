using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Toggle GodModeToggle;
    [SerializeField] Dropdown DifficultyDropdown;

    private void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {
        // Set God Mode Toggle based on PlayerPrefs
        bool godModeEnabled = PlayerPrefs.GetInt("GodMode", 0) == 1;
        GodModeToggle.isOn = godModeEnabled;

        // Set Difficulty Dropdown based on PlayerPrefs
        int selectedDifficulty = PlayerPrefs.GetInt("SelectedDifficulty", 0);
        DifficultyDropdown.value = selectedDifficulty;
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame button clicked");
        Application.Quit();
    }
}
