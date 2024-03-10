using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDemonBlastedEffect : EnergyBlastedEffect
{
    public BombDemon bombDemon;

    public override void TriggerEffect()
    {
        if (bombDemon.state != BombDemon.State.dead)
        {
            bombDemon.Exploding();
        }
    }

   

}
