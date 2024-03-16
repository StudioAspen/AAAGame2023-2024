using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-//////////////////////////////////////////////////////////////////////
///
/// Add this to a child object of any object that you want to be
/// abke to pull in like a vacuum.
/// 
public class VacuumableObject : MonoBehaviour
{
    [Header("Vacuum")]
    [SerializeField] protected float vacuumSpeed = 1;

    protected Transform player;
    public bool inVacuum { get; set; }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    protected virtual void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerInput>().transform;
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    protected virtual void Update()
    {
        InVacuumUpdate();
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    protected virtual void InVacuumUpdate()
    {
        if (inVacuum)
        {
            MoveTowards((player.position - transform.position).normalized);
        }
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    protected void MoveTowards(Vector3 argDirection)
    {
        transform.position += (argDirection * vacuumSpeed * Time.deltaTime);
    }

}
