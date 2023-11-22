using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<Level> LevelList = new List<Level>();
    [SerializeField] int CurrentLevelIndex;
    private AudioSource CurrentBGMSource;

    [System.Serializable]
    public class Level
    {
        public GameObject LevelObject;
        public AudioClip LevelBGM;   
    }
    
    void Start()
    {
        CurrentLevelIndex = 0;
        CurrentBGMSource = gameObject.AddComponent<AudioSource>();
        CurrentBGMSource.loop = true;
        PlayBGM(CurrentLevelIndex);
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
        for (int i = 0; i < LevelList.Count; i++)
        {
            if (i == CurrentLevelIndex)
            {
                LevelList[i].LevelObject.SetActive(true);
                PlayBGM(i);
            } else
            {
                LevelList[i].LevelObject.SetActive(false);
            }
        }
    }
}
