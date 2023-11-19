using UnityEngine;

public class DestroySFX : MonoBehaviour
{
    public GameObject prefabSound;
    private void OnDestroy()
    {
        //Spawn the sound object
        GameObject m_Sound = Instantiate(prefabSound, transform.position, Quaternion.identity);
        AudioSource m_Source = m_Sound.GetComponent<AudioSource>();

        float life = m_Source.clip.length / m_Source.pitch;
        Destroy(m_Sound, life);
    }
}
