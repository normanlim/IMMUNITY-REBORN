using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSampler : MonoBehaviour
{
    private Vector3 debugPosition;
    private float debugRadius;
    private List<Vector3> debugRandomPoints = new List<Vector3>();
    private Vector3 debugResult;

    public bool RandomPointAroundPosition(Vector3 position, float range, out Vector3 result, int navMeshArea = NavMesh.AllAreas)
    {
#if UNITY_EDITOR
        debugPosition = position;
        debugRadius = range;
        debugRandomPoints.Clear();
#endif

        for (int i = 0; i < 20; i++)
        {
            Vector3 randomPoint = position + Random.insideUnitSphere * range;
#if UNITY_EDITOR
            debugRandomPoints.Add(randomPoint);
#endif
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, navMeshArea))
            {
#if UNITY_EDITOR
                debugResult = hit.position;
#endif
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public bool RandomPointAroundPosition(Vector3 position, float range, float minDistanceToPosition, out Vector3 result, int navMeshArea = NavMesh.AllAreas)
    {
#if UNITY_EDITOR
        debugPosition = position;
        debugRadius = range;
        debugRandomPoints.Clear();
#endif

        for (int i = 0; i < 20; i++)
        {
            Vector3 randomPoint = position + Random.insideUnitSphere * range;
            if ((position - randomPoint).sqrMagnitude < minDistanceToPosition * minDistanceToPosition)
            {
                continue;
            }
#if UNITY_EDITOR
            debugRandomPoints.Add(randomPoint);
#endif
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, navMeshArea))
            {
#if UNITY_EDITOR
                debugResult = hit.position;
#endif
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(debugPosition, debugRadius);
        foreach (Vector3 point in debugRandomPoints)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(debugResult, 0.2f);
    }
}
