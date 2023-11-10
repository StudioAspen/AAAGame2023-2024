using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioZone : MonoBehaviour
{
    AudioSource audioSource;
    public float fadeDuration = 1.0f; // adjust to change the fade duration
    public float fullVolume = 1.0f; //adjust tp change how loud the volume should go up to (from 0 to 1)
    private bool isFading = false; // To track if fading is ongoing
    private bool hasPlayedOnce = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f; 
        audioSource.clip.LoadAudioData(); //load the data to prevent jitters upon entering a new zone
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFading) //so that there's nothing weird when quickly entering, leaving, and rentering an audio zone, we check if !isFading
        {
            StopAllCoroutines(); // Stop ongoing fade out coroutine

            if(!hasPlayedOnce) //this will ensure that the audio will only "start" during the first time entering a zone
            {
                audioSource.Play();
                hasPlayedOnce=true;
            }
            
            isFading = true;
            StartCoroutine(FadeIn());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            StopAllCoroutines(); // stop ongoing fade in coroutine
            isFading = true;
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        yield return FadeAudioSource.StartFade(audioSource, fadeDuration, fullVolume); // fade to full volume
        isFading = false;
    }

    private IEnumerator FadeOut()
    {
        yield return FadeAudioSource.StartFade(audioSource, fadeDuration, 0f); // fade to 0
        isFading = false;
    }
}