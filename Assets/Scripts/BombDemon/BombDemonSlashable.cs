using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDemonSlashable : Slashable
{
    public BombDemon BombDemon;
    public int bloodDropAmount;
    public GameObject bloodPrefab;

    public override void TriggerEffect()
    {

        //Die
        for (int i = 0; i < bloodDropAmount; i++)
        {
            Instantiate(bloodPrefab, transform.position, Quaternion.identity);
        }
            
    }



}
