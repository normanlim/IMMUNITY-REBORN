using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryAttack : MonoBehaviour
{
    // Aiming stuff
    public Transform hitSphere;
    Ray ray;
    RaycastHit hitInfo;
    public LayerMask layerMask;

    // References
    public Transform attackPoint;
    public PlayerStateMachine StateMachine;
    public GameObject muzzleFlash;

    // Projectile + Projectile force
    public GameObject memoryAtk;
    public float shootForce, upwardForce;

    // Recoil (on player)
    public float recoilForce;

    // Memory attack stuff
    bool shooting, readyToShoot, allowButtonHold;
    public float memAtkCooldown;

    public bool allowInvoke = true;


    private void Awake()
    {
        readyToShoot = true;
        allowButtonHold = false; // For future other memory attacks if we can hold down the button to channel a beam or sth;
        hitSphere.gameObject.SetActive( false );
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        if ( allowButtonHold )
        {
            // GetKey means the record the whole time the input being held down
            shooting = Input.GetKey( KeyCode.Alpha1 );
        }
        else 
        {
            // GetKeyDown means it only records the input once, even if key is being held down.
            shooting = Input.GetKeyDown( KeyCode.Alpha1 ); 
        }

        if ( readyToShoot && shooting && StateMachine.MemoryGauge.currentMeterVal >= 50 )
        {
            StateMachine.MemoryGauge.SpendMemoryGauge( 50 );
            LaunchMemoryAttack();
        }
    }

    void LaunchMemoryAttack()
    {
        readyToShoot = false;

        // set up the ray to start shooting from the camera 
        ray.origin = Camera.main.transform.position;
        ray.direction = Camera.main.transform.forward;
        float maxDist = 100.0f;
        Vector3 targetPoint;

        if ( Physics.Raycast( ray, out hitInfo, maxDist, layerMask ) )
        {
            targetPoint = hitInfo.point;
        }
        else
        {
            targetPoint = ray.GetPoint( 75 ); // somewhere far away
        }

        // Calculate direction from attackPoint to targetPoint 
        Vector3 atkDirection = targetPoint - attackPoint.position;
        GameObject currentMemoryAtk = Instantiate( memoryAtk, attackPoint.position, Quaternion.identity );
        currentMemoryAtk.transform.forward = atkDirection; // Turn the projectile to fly headfirst into direction;

        // Add forces to MemoryAtk (pew pew)
        currentMemoryAtk.GetComponent<Rigidbody>().AddForce( atkDirection.normalized * shootForce, ForceMode.Impulse );
        currentMemoryAtk.GetComponent<WeaponDamager>().SetDamage( 100, 1 );

        Destroy( currentMemoryAtk, 3f );

        //Instantiate muzzle flash, if you have one
        if ( muzzleFlash != null )
        {
            muzzleFlash.GetComponent<ParticleSystem>().Play();
        }

        //Invoke resetShot function (if not already invoked), with your timeBetweenShooting
        if ( allowInvoke )
        {
            Invoke( "ResetAttack", memAtkCooldown );
            allowInvoke = false;

            //Add recoil to player (should only be called once)
            StateMachine.ForceReceiver.AddForce( -atkDirection.normalized * recoilForce );
        }

        Debug.DrawLine( ray.origin, hitInfo.point, Color.magenta, 3f );
    }

    private void ResetAttack()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }
}
