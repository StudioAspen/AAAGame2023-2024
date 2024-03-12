using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-//////////////////////////////////////////////////////////////////////
///
/// Add this to a child object of any object that you want to be
/// abke to pull in like a vacuum.
/// 
[RequireComponent(typeof(Collider))]
public class VacuumTrigger : MonoBehaviour
{
    [SerializeField] private float vacuumRange = 1f;

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void OnValidate()
    {
        transform.localScale = new Vector3(vacuumRange, vacuumRange, vacuumRange);
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<VacuumableObject>(out VacuumableObject vacuumableObject))
        {
            vacuumableObject.inVacuum = true;
        }
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent<VacuumableObject>(out VacuumableObject vacuumableObject))
        {
            vacuumableObject.inVacuum = false;
        }
    }
}
