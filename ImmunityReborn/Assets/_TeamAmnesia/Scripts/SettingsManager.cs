using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Toggle godModeToggle;
    public Toggle softcoreToggle;
    public Dropdown difficultyDropdown;
    private int previousDifficultyIndex = -1;

    void Start()
    {
        difficultyDropdown.onValueChanged.AddListener( delegate { DifficultyChanged(); } );
        softcoreToggle.onValueChanged.AddListener( delegate { ToggleSoftcore(); });
        godModeToggle.onValueChanged.AddListener( delegate { ToggleGodMode(); } );
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
    }

    public void ToggleGodMode()
    {
        PlayerPrefs.SetInt( "GodMode", godModeToggle.isOn ? 1 : 0 );
    }

    public void ToggleSoftcore()
    {
        PlayerPrefs.SetInt("Softcore", softcoreToggle.isOn ? 1 : 0);
    }
}