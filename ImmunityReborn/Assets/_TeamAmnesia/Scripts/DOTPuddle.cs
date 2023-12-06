using System.Collections;
using UnityEngine;

public class DOTPuddle : MonoBehaviour
{
    [SerializeField] private float PuddleDPS;
    [SerializeField] private GameObject PuddleDOTPrefab;
    [SerializeField] private float PuddleDelay;

    private GameObject puddleInstance;

    public void SetPuddleDPS(float PuddleDPS)
    {
        this.PuddleDPS = PuddleDPS;
    }

    public void SpawnPuddleOnFloor()
    {
        Invoke( nameof( SpawnPuddle ), PuddleDelay ); // Spawn puddle with slight delay so we don't snapshot the damage too early
    }

    private void SpawnPuddle()
    {
        puddleInstance = Instantiate( PuddleDOTPrefab, transform.position, transform.rotation * Quaternion.Euler( -90f, 0, 0 ) );
        DOTDamager dotDamager = puddleInstance.GetComponent<DOTDamager>();
        dotDamager.SetDPS(PuddleDPS);
    }

}
