using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager GetInstance()
    {
        // if instance doesnt exist, find it in the scene
        if (instance == null)
        {
            instance = FindObjectOfType<AudioManager>();

            // if instance isn't in the scene, create a new GameObject and add this script to it
            if (instance == null)
            {
                GameObject obj = new GameObject("AudioManager");
                instance = obj.AddComponent<AudioManager>();
            }
        }
        return instance;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // placeholder function, functionality will be implemented later
    public void PlayAudio(string key)
    {
        //For testing
        Debug.Log(key);
    }
    //future stuff go here 
}
