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
        difficultyDropdown.onValueChanged.AddListener( delegate { DifficultyChanged( difficultyDropdown.value ); } );
        softcoreToggle.onValueChanged.AddListener(delegate { ToggleSoftcore(softcoreToggle.isOn); });
        godModeToggle.onValueChanged.AddListener( delegate { ToggleGodMode( godModeToggle.isOn ); } );
    }

    public void DifficultyChanged( int difficultyIndex )
    {
        if (difficultyIndex != previousDifficultyIndex)
        {
            PlayerPrefs.SetInt("SelectedDifficulty", difficultyIndex);
            PlayerPrefs.Save();

            ApplyDifficultySetting(difficultyIndex);

            previousDifficultyIndex = difficultyIndex;
        }
    }

    // Maybe you can do your difficulty implementation here, or do it in the other scene's code somewhere.
    void ApplyDifficultySetting( int difficultyIndex )
    {
        switch ( difficultyIndex )
        {
            case 0: // Easy
                //SetMonsterDamage( EasyDamageValue );
                Debug.Log( "ez difficulty" );
                break;
            case 1: // Hard
                    //SetMonsterDamage( MediumDamageValue );
                Debug.Log( "medium difficulty" );
                break;
            case 2: // Almost Impossible
                    //SetMonsterDamage( HardDamageValue );
                Debug.Log( "hard difficulty" );
                break;
        }
    }


    public void ToggleGodMode( bool isOn )
    {
        PlayerPrefs.SetInt( "GodMode", isOn ? 1 : 0 );
    }

    public void ToggleSoftcore(bool isOn)
    {
        PlayerPrefs.SetInt("Softcore", isOn ? 1 : 0);
    }
}