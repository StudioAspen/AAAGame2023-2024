using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject pathPointPrefab;
    [Space]

    [Header("Properties")]
    [SerializeField] private bool gizmosOn = true;
    [Tooltip("Enable to have moving platforms loop from last back to first path point")]
    [SerializeField] private bool isLooping = true;
    [SerializeField] private float speed = 5f;
    [Space]

    [SerializeField] private List<Transform> pathPoints = new List<Transform>();
    [SerializeField] private Transform platform;

    private Transform targetPathPoint;
    private int pathPointIndex = 0;
    private int pathPointDirection = 1; // Direction of the path points (-1 or 1)
    
    ///-////////////////////////////////////////////////////////////////
    ///
    private void Start()
    {
        targetPathPoint = pathPoints[pathPointIndex];
    }

    ///-////////////////////////////////////////////////////////////////
    ///
    private void Update()
    {
        UpdatePosition();
    }

    ///-////////////////////////////////////////////////////////////////
    ///
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.parent = platform;
        }
    }

    ///-////////////////////////////////////////////////////////////////
    ///
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }

    ///-////////////////////////////////////////////////////////////////
    ///
    private void UpdatePosition()
    {
        // Move Platform
        Vector3 direction = targetPathPoint.transform.position - platform.position;
        direction.Normalize();

        platform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Update Target Path Point if Reached Path Point
        float distance = Vector3.Distance(platform.position, targetPathPoint.position);
        if (distance <= 0.05f)
        {
            if (isLooping)
            {
                pathPointIndex += pathPointDirection;
                // Reset TargetPathPoint if reached end
                if (pathPointIndex >= pathPoints.Count)
                {
                    pathPointIndex = 0;
                }
            }
            else
            {
                // Sets path direction if hit end of path
                if (pathPointIndex >= pathPoints.Count - 1)
                {
                    pathPointDirection = -1;
                }
                else if (pathPointIndex <= 0)
                {
                    pathPointDirection = 1;
                }

                // Update Target Path Point
                pathPointIndex += pathPointDirection;
            }
            targetPathPoint = pathPoints[pathPointIndex];
        }
    }

#if UNITY_EDITOR
    #region MOVING PLATFORM GIZMOS
    ///-////////////////////////////////////////////////////////////////
    ///
    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        if (gizmosOn == false || pathPoints == null) return;

        Gizmos.color = Color.red;

        for (int i = 0; i < pathPoints.Count; i++)
        {
            // Draw point sphere
            Gizmos.DrawSphere(pathPoints[i].position, 0.05f);

            // Draw line between path points
            if (i + 1 < pathPoints.Count)
            {
                Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
            }
        }

        if (isLooping)
        {
            Gizmos.DrawLine(pathPoints[pathPoints.Count - 1].position, pathPoints[0].position);
        }
    }
    #endregion // MOVING PLATFORM GIZMOS
#endif // UNITY_EDITOR
}
