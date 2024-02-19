using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    List<Checkpoint> checkpoints = new List<Checkpoint>();
    int recentCheckpoint = -1;
    private void Start() {
        checkpoints.AddRange(GetComponentsInChildren<Checkpoint>()); // Getting all the checkpoints in the children of the checkpoint manager

        // Initalizing the values for each checkpoint according to position in heirarchy
        int currIndex = 0;
        foreach(Checkpoint checkpoint in checkpoints) {
            checkpoint.index = currIndex;
            checkpoint.checkPointManager = this;
            currIndex++;
        }
    }

    public bool CanActivateCheckpoint(int triggeredIndex) {
        if(triggeredIndex > recentCheckpoint) {
            recentCheckpoint = triggeredIndex;

            //Activating all the previous checkpoints
            for(int i = 0; i < recentCheckpoint; i++) {
                checkpoints[i].ActivateCheckpoint();
            }

            return true;
        }
        return false;
    }
}