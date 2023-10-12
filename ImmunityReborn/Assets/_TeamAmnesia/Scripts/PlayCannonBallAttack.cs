using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCannonBallAttack : MonoBehaviour
{

    public CannonBallController cannonBallController; 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cannonBallController.PlayAnimationAndParticleSystem();
        }
    }
    
}
