using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOrbsDeath : MonoBehaviour {
    public int singleBloodAmount;
    public int bloodAmountSpawned;
    public int stunnedBloodAmountSpawned;
    public GameObject bloodPrefab;

    public void SpawnOrbs() {
        /*
         * Slash and Slash Dash both kill the enemy instantly. 
         * Upon death the enemy drops a specified amount of blood and 
         * grants a temporary speed boost to the player for a specified number of seconds. 
         */
        float spawnAmount = 0;
        if (gameObject.GetComponent<SausageEnergyBlast>().isStunned) {
            spawnAmount = stunnedBloodAmountSpawned;
        }
        else {
            spawnAmount = bloodAmountSpawned;
        }

        for (int i = 0; i < spawnAmount; i++) {
            bloodPrefab.GetComponent<BloodOrb>().gainBloodAmount = singleBloodAmount;
            Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            if (gameObject.TryGetComponent(out EnemyStateManager range))
                range.Death();
            if (gameObject.TryGetComponent(out MeleeEnemyStateManager melee))
                melee.Death();
        }
    }
}
