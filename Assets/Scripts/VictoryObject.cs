using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryObject : MonoBehaviour
{
    // code determines when the win screen should be active
    
    public GameObject winScreen;

    void Start()
    {
        winScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        
        // PlayerObject would be subbed in for whatever the actual name of the player object is

        if (collision.gameObject.name == "PlayerObject")
        {
            winScreen.SetActive(true);
        }
        else
            winScreen.SetActive(false);
    }


}
