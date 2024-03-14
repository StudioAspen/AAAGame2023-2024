using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies; // List of unique enemies, enemies are spawned randomly so if you want a specific enemy to spawn from here just have that ONE enemy in this list
    [SerializeField] int numberSpawned;
    // Spawning random enemy from list
    public void SpawnEnemy() {
        for(int i = 0; i < numberSpawned; i++) {
            int index = Random.Range(0, enemies.Count);
            Instantiate(enemies[index], transform.position, Quaternion.identity);
        }
    }
}
