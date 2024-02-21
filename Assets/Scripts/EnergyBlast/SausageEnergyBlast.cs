using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SausageEnergyBlast : EnergyBlastedEffect
{
    public bool isStunned;
    public int stunTime;
    public int bloodAmount;
    public GameObject bloodPrefab;
    public GameObject bulletFirePoint;

    private EnemyStateManager enemy;
    private void Start()
    {
        enemy = gameObject.GetComponent<EnemyStateManager>();
    }

    public override void TriggerEffect()
    {
        // Energy blasts stun this enemy for a few seconds.
        // If the enemy is killed while stunned then the enemy drops an increased amount of blood.
        Debug.Log("blasted");
        if(!isStunned)
        {
            isStunned = true;
            enemy.Stun();
        }
    }
}
