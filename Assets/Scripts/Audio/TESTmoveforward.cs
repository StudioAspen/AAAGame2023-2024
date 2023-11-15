using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTmoveforward : MonoBehaviour
{
    [SerializeField]float speed = 5.0f;

    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);


        //Testing audio manager accesss and functionality
        if(Input.GetKey(KeyCode.E))
        {
            AudioManager.Instance.PlayAudio("Stab Sound");
        }
    }
}
