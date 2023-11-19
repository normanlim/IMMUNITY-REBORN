using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<GameObject> LevelList = new List<GameObject>();
    [SerializeField] int CurrentLevelIndex;

    void Start()
    {
        CurrentLevelIndex = 0;
    }

    public void StartNextLevel()
    {
        CurrentLevelIndex++;
        if (CurrentLevelIndex >= LevelList.Count)
        {
            Debug.Log("There are no more levels, nothing happens.");
            return;
        }
        for (int i = 0; i < LevelList.Count; i++)
        {
            if (i == CurrentLevelIndex)
            {
                LevelList[i].SetActive(true);
            } else
            {
                LevelList[i].SetActive(false);
            }
        }
    }
}
