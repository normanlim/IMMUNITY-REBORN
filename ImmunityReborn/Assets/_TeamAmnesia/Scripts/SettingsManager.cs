using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Toggle godModeToggle;
    public Toggle softcoreToggle;
    public Dropdown difficultyDropdown;
    private int previousDifficultyIndex = -1;
    public GUISoundManager soundManager;

    void Start()
    {

        difficultyDropdown.onValueChanged.AddListener( delegate { DifficultyChanged(); } );
        softcoreToggle.onValueChanged.AddListener( delegate { ToggleSoftcore(); });
        godModeToggle.onValueChanged.AddListener( delegate { ToggleGodMode(); } );
        soundManager = FindFirstObjectByType<GUISoundManager>();
    }

    public void DifficultyChanged()
    {
        int difficultyIndex = difficultyDropdown.value;
        if (difficultyIndex != previousDifficultyIndex)
        {
            PlayerPrefs.SetInt("SelectedDifficulty", difficultyIndex);
            PlayerPrefs.Save();

            previousDifficultyIndex = difficultyIndex;
        }
        if (difficultyDropdown.isActiveAndEnabled)
            soundManager.PlaySelectConfirmation();
    }

    public void ToggleGodMode()
    {
        PlayerPrefs.SetInt( "GodMode", godModeToggle.isOn ? 1 : 0 );
        if (godModeToggle.isActiveAndEnabled)
            soundManager.PlaySelectConfirmation();
    }

    public void ToggleSoftcore()
    {
        PlayerPrefs.SetInt("Softcore", softcoreToggle.isOn ? 1 : 0);
        if (softcoreToggle.isActiveAndEnabled)
            soundManager.PlaySelectConfirmation();
    }
}