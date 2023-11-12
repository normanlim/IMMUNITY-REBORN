using System.Collections;
using UnityEngine;

public class DOTPuddle : MonoBehaviour
{
    [SerializeField] private GameObject PuddleDOTPrefab;
    [SerializeField] private float PuddleLifetime;
    [SerializeField] private float PuddleDelay;

    private GameObject player;
    private GameObject puddleInstance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag( "Player" );
    }
    public void SpawnPuddleOnFloor()
    {
        Invoke( nameof( SpawnPuddle ), PuddleDelay ); // Spawn puddle with slight delay so we don't snapshot the damage too early
    }

    private void SpawnPuddle()
    {
        puddleInstance = Instantiate( PuddleDOTPrefab, transform.position, transform.rotation * Quaternion.Euler( -90f, 0, 0 ) );
        Invoke( nameof( DestroyPuddle ), 5f ); // Destroy the instance after 10 seconds
    }

    private void DestroyPuddle()
    {
        if ( puddleInstance != null )
        {
            Destroy( puddleInstance );
        }
    }
}
