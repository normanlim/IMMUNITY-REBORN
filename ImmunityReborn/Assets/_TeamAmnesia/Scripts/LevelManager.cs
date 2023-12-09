using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int CurrentLevelIndex;
    private int currentLevelIndex = -1; // backing field
    [SerializeField] int AdditionalHealCount = 3;
    [SerializeField] List<Level> LevelList = new List<Level>();

    [SerializeField] private AudioSource CurrentBGMSource;
    // References used to move player gracefully
    [SerializeField] private GameObject Player;
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private Cloth playerCape;

    [System.Serializable]
    public class Level
    {
        public GameObject LevelObject;
        public GameObject LevelStart;
        public AudioClip LevelBGM;   
    }

    void Start()
    {
        // Start at level 1
        // CurrentLevelIndex = 0;
        StartLevel();
    }

    // Useful for internal testing as you can set the level to test in Unity editor
    private void OnValidate()
    {
        if (Application.isPlaying && CurrentLevelIndex != currentLevelIndex && CurrentLevelIndex >= 0 && CurrentLevelIndex < LevelList.Count)
        {
            StartLevel();
            currentLevelIndex = CurrentLevelIndex;
        }
    }

    public void PlayBGM(int index)
    {
        if (index >= 0 && index < LevelList.Count)
        {
            CurrentBGMSource.clip = LevelList[index].LevelBGM;
            CurrentBGMSource.volume = 0.3f;
            CurrentBGMSource.Play();
        }
        else
        {
            Debug.LogError("Invalid BGM index");
        }
    }

    public void StopBGM()
    {
        CurrentBGMSource.Stop();
    }

    public void StartNextLevel()
    {
        CurrentLevelIndex++;
        if (CurrentLevelIndex >= 0 && CurrentLevelIndex >= LevelList.Count)
        {
            Debug.Log("Invalid Level index");
            return;
        }

        StartLevel();
    }

    private void StartLevel()
    {
        for (int i = 0; i < LevelList.Count; i++)
        {
            if (i == CurrentLevelIndex)
            {
                LevelList[i].LevelObject.SetActive(true);
                PlayBGM(i);
                MovePlayerToTransform(LevelList[i]);
            }
            else
            {
                LevelList[i].LevelObject.SetActive(false);
            }
        }
        Player.GetComponent<HealthConsumable>().AddItemCount(AdditionalHealCount);
        Health PlayerHealth = Player.GetComponent<Health>();
        PlayerHealth.Heal(PlayerHealth.MaxHealth);
    }

    private void MovePlayerToTransform(Level level)
    {
        playerCape.enabled = false;
        Player.gameObject.transform.position = level.LevelStart.transform.position;
        Player.gameObject.transform.rotation = level.LevelStart.transform.rotation;
        freeLookCamera.m_XAxis.Value = level.LevelStart.transform.eulerAngles.y;
        
        playerCape.enabled = true;
    }
}
