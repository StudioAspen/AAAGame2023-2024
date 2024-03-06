using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-//////////////////////////////////////////////////////////////////////
///
/// Add this to a child object of any object that you want to be
/// abke to pull in like a vacuum.
/// 
[RequireComponent(typeof(Collider))]
public class Vacuumable : MonoBehaviour
{
    [SerializeField] private float vacuumSpeed = 2f;
    [SerializeField] private float vacuumRange = 1f;

    private Transform mainObject;

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void Awake()
    {
        mainObject = transform.parent;
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void OnValidate()
    {
        transform.localScale = new Vector3(vacuumRange, vacuumRange, vacuumRange);
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void OnTriggerStay(Collider other)
    {
        PlayerKillable player = other.transform.GetComponent<PlayerKillable>();
        if (player != null)
        {
            Vector3 direction = Vector3.ProjectOnPlane(player.transform.position - transform.position, Vector3.up);
            MoveTowards(direction);
        }
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void MoveTowards(Vector3 direction)
    {
        mainObject.Translate(direction * vacuumSpeed * Time.deltaTime);
    }
}
