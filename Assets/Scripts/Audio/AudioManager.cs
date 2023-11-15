using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject); 
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
