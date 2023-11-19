using System.Collections;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] bgmClips;
    private AudioSource audioSource;
    private int index;
    void Start()
    {
        index = 0;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true; // Set to true if you want the BGM to loop
        PlayBGM(index);
    }

    public void PlayBGM(int index)
    {
        if (index >= 0 && index < bgmClips.Length)
        {
            audioSource.clip = bgmClips[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Invalid BGM index");
        }
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }
}
