using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDemonSlashable : Slashable
{
    public BombDemon bombDemon;
    public GameObject bloodPrefab;
    public int bloodDropAmount;
   

    public override void TriggerEffect()
    {

        //Die
        if (bombDemon.state != BombDemon.State.dead)
        {
            for (int i = 0; i < bloodDropAmount; i++)
            {
                Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            }

        }

    }  
}
