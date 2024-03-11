using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BloodGague : MonoBehaviour
{
    [SerializeField] Slider currentBlood;
    [SerializeField] Slider maxBloodHandle;
    [SerializeField] Slider bloodThirstThreshold;
    [SerializeField] Slider bloodCost; 

    [SerializeField] BloodThirst bloodThirst;
    [SerializeField] EnergyBlast energyBlast;

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
        bloodCost.value = (bloodThirst.currentBlood - energyBlast.bloodPerShot) / bloodThirst.maxBloodForOverfed;
        currentBlood.value = bloodThirst.currentBlood / bloodThirst.maxBloodForOverfed;
    }
}
