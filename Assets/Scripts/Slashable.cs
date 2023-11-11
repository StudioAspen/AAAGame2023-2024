using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slashable : MonoBehaviour
{

    public Vector3 slideDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        //makes it so the gizmos transform with the local transform of the object
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(-slideDir, slideDir);

    }
}
