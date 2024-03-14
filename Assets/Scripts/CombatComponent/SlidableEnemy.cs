using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class SlidableEnemy : MonoBehaviour
{
    public PathCreator pathCreator;

    private void Start() {
        pathCreator = GetComponent<PathCreator>();
    }
    public void Die() {
        if (gameObject.TryGetComponent(out EnemyStateManager range)) { 
            if (range.currentState != range.deathState) {
                GetComponent<SpawnOrbsDeath>().SpawnOrbs();
            }
        }
        if (gameObject.TryGetComponent(out MeleeEnemyStateManager melee)) {
            if (melee.currentState != melee.deathState) {
                GetComponent<SpawnOrbsDeath>().SpawnOrbs();
            }
        }
    }
}
