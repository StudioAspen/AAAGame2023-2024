using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeMeshesManager : MonoBehaviour
{

    Transform level;
    Transform[] meshArray;

    // Start is called before the first frame update
    void Start()
    {
        //initializes transform variables
        level = transform;
        meshArray = new Transform[level.childCount];

        //gets all the meshes that need to be processed for optimization and adds them into an array
        for (int mesh = 0; mesh < level.childCount; mesh++)
        {
            meshArray[mesh] = level.GetChild(mesh);
        }

        MergeMeshes();
    }

    //merge the meshes of the same material property
    void MergeMeshes()
    {
        //creates an array that keeps track of the materials that have already been merged into one mesh
        List<Material> finishedMaterials = new List<Material>();
        Material currentMaterial;

        //loops through all the meshes for merging
        for (int mesh = 0; mesh < meshArray.Length; mesh++)
        {
            currentMaterial = meshArray[mesh].GetComponent<MeshRenderer>().material; //gets the current meshes material
            
            // checks if the meshes of this material have been merged yet
            if (!finishedMaterials.Contains<Material>(currentMaterial)) { 
                
            }
        }
        

        
    }
}
