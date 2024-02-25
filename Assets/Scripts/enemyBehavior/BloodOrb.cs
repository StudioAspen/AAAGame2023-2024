using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodOrb : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] public float gainBloodAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BloodThirst bloodThirst))
        {
            bloodThirst.GainBlood(gainBloodAmount, true);
            Destroy(gameObject);
        }
    }
}