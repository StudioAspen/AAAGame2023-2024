using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeMeshesManager : MonoBehaviour
{

    Transform level;

    // Start is called before the first frame update
    void Start()
    {
        level = transform;

        MergeMeshes();
    }

    //merge the meshes of the same material property
    void MergeMeshes()
    {

    }
}
