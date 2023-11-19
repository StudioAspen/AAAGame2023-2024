using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPool : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float gainBloodAmount;
    [SerializeField] float gainTickRate;
    [SerializeField] bool canOverfeed;

    float gainTickTimer;

    private void OnTriggerStay(Collider other) {
        if (other.TryGetComponent(out BloodThirst bloodThirst)) {
            Debug.Log("test");
            gainTickTimer -= Time.deltaTime;
            if(gainTickTimer <= 0) {
                Debug.Log("gain");
                bloodThirst.GainBlood(gainBloodAmount, canOverfeed);
                gainTickTimer = gainTickRate;
            }
        }
    }
}
