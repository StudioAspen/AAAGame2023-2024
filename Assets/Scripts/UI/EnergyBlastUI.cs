using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyBlastUI : MonoBehaviour
{
    [SerializeField] TMP_Text charges;
    [SerializeField] EnergyBlast energyBlast;

    private void Start()
    {
        energyBlast.OnChargesChanged.AddListener(UpdateCharges);
    }

    private void UpdateCharges()
    {
        charges.text = energyBlast.currNumOfCharges.ToString() + " / " + energyBlast.maxNumOfCharges.ToString();
    }
}
