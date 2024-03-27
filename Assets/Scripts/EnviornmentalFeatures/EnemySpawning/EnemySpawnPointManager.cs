using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPointManager : MonoBehaviour
{
    [SerializeField] float rangeCheck; // how far this manager is checking for enemies
    [SerializeField] int enemyCountThreshold; // Number of enemies that are required to start spawning more
    [SerializeField] LayerMask enemyLayer; // Layer the enemy is on
    [SerializeField] float checkTimer; // Time time between checks
    float timer = 0f;
    List<EnemySpawnPoint> spawnPoints = new List<EnemySpawnPoint>();

    private void Start() {
        FindSpawnPointChildren();
    }

    private void Update() {
        timer -= Time.deltaTime;
        if(timer <= 0f) {
            // Checking number of enemies within area and spawning accordingly
            if(CheckNumberOfEnemies() <= enemyCountThreshold) {
                SpawnEnemies();
            }
            timer = checkTimer;
        }
    }

    // Checking the number of enemies in a specified area
    private int CheckNumberOfEnemies() {
        return Physics.OverlapSphere(transform.position, rangeCheck, enemyLayer).Length;
    }

    // Spawning enemies from spawn points
    private void SpawnEnemies() {
        foreach(EnemySpawnPoint point in spawnPoints) {
            point.SpawnEnemy();
        }
    }

    // Getting the spawnpoints in children
    private void FindSpawnPointChildren() {
        for(int i = 0; i < transform.childCount; i++) {
            if(transform.GetChild(i).TryGetComponent(out EnemySpawnPoint point)) {
                spawnPoints.Add(point);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, rangeCheck);        
    }
}
