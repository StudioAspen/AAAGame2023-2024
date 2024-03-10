using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTsfx : MonoBehaviour
{
    [SerializeField]float volume = 1.0f;
    float speed = 5.0f;
    [SerializeField] float range = 500.0f;

    AudioSource audioSource;

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            Debug.Log("1 key pressed, global sfx test");
            AudioManager.GetInstance().PlayGlobalAudio("TESTsfx", volume);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)){
            Debug.Log("2 key pressed, spatial sfx test");
            audioSource = AudioManager.GetInstance().PlayAudioFollowObject("TESTsfx", gameObject, volume, false, range);
        }

        if(Input.GetKey(KeyCode.D)){
            transform.Translate(speed * Time.deltaTime, 0,0);
        }

        if(Input.GetKeyDown(KeyCode.Alpha4)){
            Debug.Log("4 key pressed, all test sfx instances stop");
            AudioManager.GetInstance().StopAudioOfType("TESTsfx");
        }


        if(Input.GetKeyDown(KeyCode.Alpha5)){
            Debug.Log("5 key pressed, invalid sound-name test");
            AudioManager.GetInstance().PlayGlobalAudio("IncorrectNameExample");
        }

        if(Input.GetKeyDown(KeyCode.Alpha6)){
            Debug.Log("6 key pressed, looping global sfx test");
            AudioManager.GetInstance().PlayGlobalAudio("TESTsfx", volume, true);
        }
    }

}