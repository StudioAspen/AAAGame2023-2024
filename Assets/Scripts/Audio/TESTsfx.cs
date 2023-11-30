using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTsfx : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)){
            Debug.Log("Q key pressed, global sfx test");
            AudioManager.GetInstance().PlayAudio("TESTsfx");
        }

        if(Input.GetKeyDown(KeyCode.W)){
            Debug.Log("W key pressed, spatial sfx test");
            AudioManager.GetInstance().PlayAudio("TESTsfx", transform.position);
        }

        if(Input.GetKeyDown(KeyCode.E)){
            Debug.Log("E key pressed, invalid sound-name test");
            AudioManager.GetInstance().PlayAudio("incorrectName", transform.position);
        }
    }
}