using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnergyBlastUI : MonoBehaviour
{
    [SerializeField] TMP_Text charges;
    [SerializeField] Slider slider;
    [SerializeField] EnergyBlast energyBlast;

    private void Start()
    {
       // energyBlast.OnChargesChanged.AddListener(UpdateCharges);
       //energyBlast.OnChargeTimer.AddListener(UpdateChargeTimer);
    }

    private void UpdateCharges()
    {
       // charges.text = energyBlast.currNumOfCharges.ToString() + " / " + energyBlast.maxNumOfCharges.ToString();
    }
    private void UpdateChargeTimer() {
       // slider.value = energyBlast.currRechargeTimer / energyBlast.rechargeTimer;
    }
}
