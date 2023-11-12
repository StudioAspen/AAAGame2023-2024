using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BloodGague : MonoBehaviour
{
    [SerializeField] Slider currentBlood;
    [SerializeField] Slider maxBloodHandle;
    [SerializeField] Slider bloodThirstThreshold;
    [SerializeField] BloodThirst bloodThirst;


    private void Start()
    {
        bloodThirst.OnBloodChange.AddListener(UpdateBar);
        UpdateThresholds();
    }
    public void UpdateThresholds()
    {
        maxBloodHandle.value = bloodThirst.maxBlood / bloodThirst.maxBloodForOverfed;
        bloodThirstThreshold.value = bloodThirst.bloodThirstThreshold / bloodThirst.maxBloodForOverfed;
    }

    public void UpdateBar()
    {
        currentBlood.value = bloodThirst.currentBlood / bloodThirst.maxBloodForOverfed;
    }
}
