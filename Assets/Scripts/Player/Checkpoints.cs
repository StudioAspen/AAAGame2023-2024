using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    /*----------------------------------------------
     Script is to be added to the Player GameObject
    ----------------------------------------------*/

    Transform currentCheckpoint; // the most recent checkpoint that the players

    void OnTriggerEnter(Collider checkpoint)
    {
        if (checkpoint.transform.name.Contains("Checkpoint")) // checks if the collider is a checkpoint
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
