using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    /*----------------------------------------------
     Script is to be added to the Checkpoint Manager Prefab
    ----------------------------------------------*/
    // place the checkpoint manager prefab into the scene, and any actual checkpoint prefabs inside of this manager.

    Transform currentCheckpoint; // the most recent checkpoint that the players
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerKillable>().OnDie.AddListener(RespawnPlayer);
    }

    void OnTriggerEnter(Collider checkpoint)
    {
        if (checkpoint.transform.CompareTag("Checkpoint"))// checks if the collider is a checkpoint
        {
            // sets the most recent checkpoint
            currentCheckpoint = checkpoint.transform;
        }
    }

    // call RespawnPlayer() in order to reset its position to the checkpoint
    public void RespawnPlayer()
    {
        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position; //sets player checkpoint
        }
    }
}
