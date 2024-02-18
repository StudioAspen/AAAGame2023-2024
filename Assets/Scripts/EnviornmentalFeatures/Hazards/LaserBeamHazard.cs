using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

///-////////////////////////////////////////////////////////////////
///
public class LaserBeamHazard : MonoBehaviour
{
    [SerializeField] private GameObject pathPointPrefab;
    [Space]
    
    [Header("Properties")]
    [SerializeField] private bool gizmosOn = true;
    [Tooltip("Enable to have laserbeam loop from last back to first path point")]
    [SerializeField] private bool isLooping = true;
    [Tooltip("Damage to blood")]
    [SerializeField] private float damage = 5f;
    [SerializeField] private float speed = 5f;

    [SerializeField] private List<Transform> pathPoints = new List<Transform>();
    private Transform laserBeam;

    private Transform targetPathPoint;
    private int pathPointIndex = 0;
    private int pathPointDirection = 1; // Direction of the path points (-1 or 1)

    ///-////////////////////////////////////////////////////////////////
    ///
    void Start()
    {
        laserBeam = transform.Find("LaserBeam");
        if (laserBeam == null)
        {
            Debug.LogError("Laser beam must be named 'LaserBeam' and must be a child of the prefab");
        }
        targetPathPoint = pathPoints[pathPointIndex];
    }

    ///-////////////////////////////////////////////////////////////////
    ///
    void Update()
    {
        UpdateLaserBeamPosition();
    }

    ///-////////////////////////////////////////////////////////////////
    ///
    private void OnCollisionEnter(Collision collision)
    {
        BloodThirst player = collision.transform.GetComponent<BloodThirst>();

        if (player != null)
        {
            // Damage player
            player.LoseBlood(damage);
        }
    }

    ///-////////////////////////////////////////////////////////////////
    ///
    private void UpdateLaserBeamPosition()
    {
        // Move Laser Beam
        Vector3 direction = targetPathPoint.transform.position - laserBeam.transform.position;
        direction.Normalize();

        laserBeam.Translate(direction *  speed * Time.deltaTime, Space.World);
        laserBeam.forward = direction;
        laserBeam.Rotate(new Vector3(0, 0, 90));

        // Update Target Path Point if Reached Path Point
        float distance = Vector3.Distance(laserBeam.position, targetPathPoint.position);
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
    #region LASER BEAM GIZMOS
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
    #endregion // LASER BEAM GIZMOS
#endif // LASER BEAM GIZMOS
}
