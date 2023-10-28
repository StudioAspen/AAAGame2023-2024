using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BloodThirst : MonoBehaviour
{
    
    [SerializeField] float bloodThirstThreshold;
    [SerializeField] public float maxBlood;
    float currentBlood;
    float maxBloodForBoost;
    float maxAmount;

    float drainRate;
    float overfedDrainRate;


    UnityEvent OnBloodChange; // Sends signal update to the UI

    public void GainBlood(float amount)
    {
        currentBlood += amount;
        OnBloodChange.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentBlood -= drainRate * Time.deltaTime;
        OnBloodChange.Invoke();
    }
}
