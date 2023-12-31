using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogWallHandler : MonoBehaviour
{
    private enum FogWallType
    {
        Block,
        Pass
    }

    [SerializeField]
    private BoxCollider fogWall;
    [SerializeField]
    private FogWallType changeToType;
    [SerializeField]
    private Material originalMaterial;
    [SerializeField]
    private Material changeToMaterial;

    private void OnEnable()
    {
        
        // Reset fog wall
        if (fogWall.TryGetComponent(out MeshRenderer meshRenderer))
        {
            meshRenderer.material = originalMaterial;
        }
        fogWall.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (changeToType == FogWallType.Block)
            {
                fogWall.isTrigger = false;
            }
            else if (changeToType == FogWallType.Pass)
            {
                fogWall.isTrigger = true;
            }

            if (fogWall.TryGetComponent(out MeshRenderer meshRenderer))
            {
                meshRenderer.material = changeToMaterial;
            }
        }
    }
}
