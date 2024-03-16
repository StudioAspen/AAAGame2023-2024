using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class SlidableEnemy : MonoBehaviour
{
    public PathCreator pathCreator;
    [Header("Adjustable")]
    [SerializeField] float movementBonus; // How much faster things move

    private void Start() {
        pathCreator = GetComponent<PathCreator>();
    }

    public float GetBonus() {
        if(CheckStunned()) {
            return movementBonus;
        }
        else {
            return 0;
        }
    }
    public bool CheckStunned() {
        if (gameObject.TryGetComponent(out EnemyStateManager range)) {
            if (range.currentState == range.stunState) {
                return true;
            }
        }
        if (gameObject.TryGetComponent(out MeleeEnemyStateManager melee)) {
            if (melee.currentState == melee.stunState) {
                return true;
            }
        }
        return false;
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
