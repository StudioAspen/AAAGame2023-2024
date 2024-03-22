using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Level2")
        {
            SceneManager.LoadScene(1);
        }

        if (other.tag == "Restart1")
        {
            SceneManager.LoadScene(2);
        }
    }
}

