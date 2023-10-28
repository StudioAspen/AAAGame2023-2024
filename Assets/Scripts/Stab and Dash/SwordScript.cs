using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("Dashable"))
        {
            Debug.Log("DASH!");
            transform.parent.GetComponent<Dash>().OnTargetHit(this);
            transform.parent.GetComponent<Stab>().InterruptStab();
        }

    }

}
