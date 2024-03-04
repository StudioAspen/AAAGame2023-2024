using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAudioSource : MonoBehaviour
{
    public Transform target;

    //this script will be added to audio sources called by PlayAudioFollowObject(), 
    //which will make the audio source follow the object that called the function without
    // making the audio source a child of the object (which can cause issues)

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
        else
        {
            // Target is gone, stop the sound and destroy this object
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Stop();
            }
            Destroy(gameObject);
        }
    }
}
