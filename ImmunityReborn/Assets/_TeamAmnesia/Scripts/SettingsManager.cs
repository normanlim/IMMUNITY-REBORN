using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Toggle godModeToggle;
    public Dropdown difficultyDropdown;

    void Start()
    {
        godModeToggle.onValueChanged.AddListener( delegate { ToggleGodMode( godModeToggle.isOn ); } );
        difficultyDropdown.onValueChanged.AddListener( delegate { DifficultyChanged( difficultyDropdown.value ); } );
    }

    public void DifficultyChanged( int difficultyIndex )
    {
        PlayerPrefs.SetInt( "SelectedDifficulty", difficultyIndex );
        PlayerPrefs.Save();

        ApplyDifficultySetting( difficultyIndex );

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
}