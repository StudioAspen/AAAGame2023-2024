using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashThroughEnemy : StabableDashThrough
{
    [SerializeField] float inputDashLength; // how far past the enemy you 
    [SerializeField] public float movementBonus; // speed added on top of player speed calcuations
    public override void CalculateDash(GameObject source) {
        dashDir = source.transform.forward;
        Debug.DrawLine(transform.position, transform.position + dashDir, Color.blue);
        dashLength = inputDashLength;
    }
    public float GetBonus() {
        if(CheckStunned()) {
            return movementBonus;
        }
        return 0;
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
