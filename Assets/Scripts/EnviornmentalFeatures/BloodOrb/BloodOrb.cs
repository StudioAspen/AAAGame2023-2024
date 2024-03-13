using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodOrb : VacuumableObject
{
    [Header("Blood Orb")]
    [SerializeField] public float gainBloodAmount;

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BloodThirst bloodThirst))
        {
            bloodThirst.GainBlood(gainBloodAmount, true);
            Destroy(gameObject);
        }
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    protected override void InVacuumUpdate()
    {
        base.InVacuumUpdate();
    }
}