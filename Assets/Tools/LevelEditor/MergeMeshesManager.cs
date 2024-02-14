using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GetMikyled.LevelEditor
{
    public class MergeMeshesManager : MonoBehaviour
    {

        Transform level; // the parent game object of all game objects in the level.
        Transform[] meshArray; // contains all the meshes created using the level editor
        List<Material> materialsToBeMerged; // used to keep track of materials of meshes that have already been merged
        CombineInstance[] combinedMeshes; // contains the CombineInstance that will hold all meshes to be combined on run time
        Mesh newMesh; // new combined mesh

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
            materialsToBeMerged = new List<Material>();
            Material currentMaterial;

            //loops through all the meshes to check what materials need to be merged together
            for (int mesh = 0; mesh < meshArray.Length; mesh++)
            {
                currentMaterial = meshArray[mesh].GetComponent<MeshRenderer>().material; //gets the current meshes material

                bool uniqueMaterial = true;

                // checks if the meshes of this material have been added to the list yet
                for (int material = 0; material < materialsToBeMerged.Count; material++)
                {
                    if (materialsToBeMerged[material].name == currentMaterial.name) { uniqueMaterial = false; }
                }

                if (uniqueMaterial)
                {
                    materialsToBeMerged.Add(currentMaterial);
                }
            }

            //merge meshes of same material
            for (int material = 0; material < materialsToBeMerged.Count; material++)
            {
                // creates a new list that contains all the meshes of the current material
                List<Transform> tempList = new List<Transform>();
                for (int mesh = 0; mesh < meshArray.Length; mesh++)
                {
                    if (meshArray[mesh].GetComponent<MeshRenderer>().material.name == materialsToBeMerged[material].name)
                    {
                        tempList.Add(meshArray[mesh]);
                    }
                }
                combinedMeshes = new CombineInstance[tempList.Count]; //combined instance for method CombineMeshes()

                // gets the meshes and transforms for combining, then destroys it
                for (int mesh = 0; mesh < tempList.Count; mesh++)
                {
                    combinedMeshes[mesh].mesh = tempList[mesh].GetComponent<MeshFilter>().mesh;
                    combinedMeshes[mesh].transform = tempList[mesh].GetComponent<MeshFilter>().transform.localToWorldMatrix;
                    Destroy(tempList[mesh].gameObject);
                }

                // creates the new optimized game object
                newMesh = new Mesh();
                newMesh.CombineMeshes(combinedMeshes);
                GameObject newGameObject = new GameObject();
                newGameObject.transform.SetParent(level.transform);
                newGameObject.name = materialsToBeMerged[material].name;
                MeshRenderer renderer = newGameObject.AddComponent<MeshRenderer>();
                MeshFilter filter = newGameObject.AddComponent<MeshFilter>();
                renderer.material = materialsToBeMerged[material];
                filter.mesh = newMesh; ;
            }
        }
    }

}