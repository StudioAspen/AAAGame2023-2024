using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashThroughEnemy : StabableDashThrough
{
    [SerializeField] float inputDashLength;
    public override void CalculateDash(GameObject source) {
        dashDir = source.transform.forward;
        Debug.DrawLine(transform.position, transform.position + dashDir, Color.blue);
        dashLength = inputDashLength;
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
