using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    public static void PlayThenDestroy(GameObject soundPrefab, Transform transform)
    {
        // Spawn the sound object
        GameObject m_Sound = Instantiate(soundPrefab, transform.position, Quaternion.identity);
        AudioSource m_Source = m_Sound.GetComponent<AudioSource>();

        float life = m_Source.clip.length / m_Source.pitch;
        Destroy(m_Sound, life);
    }

    public static GameObject PlayWithLoop(GameObject soundPrefab, Transform transform)
    {
        // Spawn the sound object
        GameObject soundObject = Instantiate(soundPrefab, transform.position, Quaternion.identity);
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();

        // Set the AudioSource to loop
        audioSource.loop = true;

        return soundObject;
    }

    // Example method to stop a looped audio source manually
    public static void StopLoopedAudio(GameObject soundObject)
    {
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = false; // Disable loop
            Destroy(audioSource.gameObject, audioSource.clip.length / audioSource.pitch); // Destroy after the clip finishes
        }
    }
}
