using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    public static void PlayThenDestroy(GameObject soundPrefab, Transform transform)
    {
        //Spawn the sound object
        GameObject m_Sound = Instantiate(soundPrefab, transform.position, Quaternion.identity);
        AudioSource m_Source = m_Sound.GetComponent<AudioSource>();

        float life = m_Source.clip.length / m_Source.pitch;
        Destroy(m_Sound, life);
    }
}
