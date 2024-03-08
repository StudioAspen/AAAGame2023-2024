using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickEnemyBlastedEffect : EnergyBlastedEffect
{
    FlickEnemyStabable stabable;

    private void Start() {
        stabable = GetComponent<FlickEnemyStabable>();
    }
    public override void TriggerEffect() {
        stabable.Stun();
    }
}
