using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //singleton
    static AudioManager instance;

    public static AudioManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<AudioManager>();

            if (instance == null)
            {
                GameObject obj = new GameObject("AudioManager");
                instance = obj.AddComponent<AudioManager>();
            }
        }
        return instance;
    }

    //dictionaries are a collection of items with two values, a key and a value
    //in this context, we're creating a dictionary called audioClips, 
    //where the values are the audioclip, and the key is the string that points to it

    private Dictionary<string, AudioClip> audioClips;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        LoadAudioClips();
    }


    //thr current implementation depends on there being an "Audio" Folder inside of a 
    //"Resources" folder to take advantage of the Resources.LoadAll method
    void LoadAudioClips()
    {
        //initializes a new, empty dictionary
        audioClips = new Dictionary<string, AudioClip>(); 

        //declares an array of AudioClips, takes advantage of the
        //Resources.LoadAll method to add all audioclips into the array
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");

        //for every audioclip in the array, create an entry in the dictionary
        foreach(var clip in clips){
            audioClips.Add(clip.name, clip);
        }
    }

    public void PlayAudio(string key, Vector3 position = default)
    {
        if (audioClips.ContainsKey(key))
        {
            if (position == Vector3.zero){
                audioSource.PlayOneShot(audioClips[key]);
            }
            else{
                AudioSource.PlayClipAtPoint(audioClips[key], position);
            }
        }
        else{
            Debug.LogWarning("Audio clip not found: " + key);
        }
    }
    //each instance of sound can be called by at the location of an object's script using 
    //"AudioManager.GetInstance().PlayAudio("SoundFileName", transform.position)"
    //or
    //for audio not coming from a particular location(Music or UI)
    //"AudioManager.GetInstance().PlayAudio("SoundFileName")"
}
