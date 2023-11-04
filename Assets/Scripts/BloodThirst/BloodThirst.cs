using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BloodThirst : MonoBehaviour
{
    
    [SerializeField] public float bloodThirstThreshold; // Threshold for blood thirst
    [SerializeField] public float maxBlood; // Max amount of blood before overfed
    public float currentBlood; // Current blood amount
    float maxBloodForOverfed; // Max amount of blood for overfed

    float drainRate; // Blood drain rate
    float overfedDrainRate; // Blood drain rate when overfed


    UnityEvent OnBloodChange = new UnityEvent(); // Sends signal update to the UI
    MovementModification movementModification;

    // Start is called before the first frame update
    void Start()
    {
        if(TryGetComponent<MovementModification>(out movementModification))
        {
            Debug.Log("Movement modification not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBlood < maxBlood) {
            currentBlood -= drainRate * Time.deltaTime;
        }
        else
        {
            currentBlood -= overfedDrainRate * Time.deltaTime;
        }
        OnBloodChange.Invoke();

        if (movementModification != null)
        {
            // Setting boost based on thresholds but not letting it drop below zero
            movementModification.SetBoost(Mathf.Max(Mathf.InverseLerp(maxBlood, maxBloodForOverfed, currentBlood), 0));
        }
    }


    public void GainBlood(float amount)
    {
        currentBlood += amount;
        OnBloodChange.Invoke();
    }
}
