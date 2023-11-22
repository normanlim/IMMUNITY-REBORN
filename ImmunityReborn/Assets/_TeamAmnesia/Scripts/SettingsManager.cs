using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Toggle godModeToggle;

    void Start()
    {
        godModeToggle.onValueChanged.AddListener( delegate { ToggleGodMode( godModeToggle.isOn ); } );
    }

    public void ToggleGodMode( bool isOn )
    {
        PlayerPrefs.SetInt( "GodMode", isOn ? 1 : 0 );
    }
}