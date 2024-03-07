using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTsfx2 : MonoBehaviour
{
    [SerializeField]float volume = 1.0f;
    float speed = 5.0f;
    [SerializeField] float range = 500.0f;

    AudioSource audioSource;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            Debug.Log("2 key pressed, spatial sfx test");
            audioSource = AudioManager.GetInstance().PlayAudioFollowObject("TESTsfx", gameObject, volume, false, range);
        }

        if(Input.GetKey(KeyCode.D)){
            transform.Translate(speed * Time.deltaTime, 0,0);
        }

        if(Input.GetKeyDown(KeyCode.Alpha3)){
            Debug.Log("3 key pressed, stopping right audio source");
            AudioManager.GetInstance().StopAudio(audioSource);
        }
    }
}
