using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    /*----------------------------------------------
     Script is to be added to checkpoint prefab
    ----------------------------------------------*/
    // place the checkpoint manager prefab into the scene, and any actual checkpoint prefabs inside of this manager.
    public int index; // Set by the checkpoint manager based on index in heirarchy
    public bool triggered = false; // Set by the manager
    public CheckPointManager checkPointManager; // Used to check if they can be used as checkpoint
    public Renderer render; // Temperary component use for testing

    private void Start() {
        render = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.CompareTag("Player"))// checks if the collider is a checkpoint
        {
            if(checkPointManager.CanActivateCheckpoint(index)) {
                render.material.color = Color.blue;
                collider.transform.GetComponent<PlayerKillable>().respawnPosition = transform.position; // setting player respawn position if valid checkpoint
            }
        }
    }

    public void ActivateCheckpoint() {
        triggered = true;
        render.material.color = Color.cyan; // Temperary visual to show it has been activated
    }
}
