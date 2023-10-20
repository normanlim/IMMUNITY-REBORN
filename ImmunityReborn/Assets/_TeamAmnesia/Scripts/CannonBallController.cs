using UnityEngine;

public class CannonBallController : MonoBehaviour
{
    public Vector3 targetPosition; 
    public float animationDuration;
    public ParticleSystem ps;

    private float elapsedTime = 0f;
    private Vector3 initialPosition;
    private bool animationRunning = false;

    private void Start()
    {
        initialPosition = transform.position;
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void Update()
    {
        if (animationRunning)
        {
            if (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
                elapsedTime += Time.deltaTime;
            }
            else
            {
                transform.position = initialPosition;
                animationRunning = false; 
            }
        }
    }

    public void PlayAnimationAndParticleSystem()
    {
        if (!animationRunning)
        {
            GetComponent<MeshRenderer>().enabled = true;
            ps.Play();

            transform.position = initialPosition;
            elapsedTime = 0f;

            animationRunning = true;
        }
    }
}