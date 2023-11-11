using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Unity.VisualScripting;
using UnityEditor.Rendering;

public class LevelEditor : EditorWindow
{
    GameObject level; //stores the gameobject level, which is the parent of all of the prefabs

    string prefabFolderPath = "Assets/Prefabs";
    string[] prefabFolders;
    int selected = 0; // sets default popup value

    Texture2D prefabTexture;
    GameObject previewPrefab;

    //tool state
    GameObject selectedPrefab;
    Boolean eraserActive = false;


    [MenuItem("Tools/Level Editor")]
    public static void ShowWindow() // Show the Level Editor Window
    {
        EditorWindow.GetWindow<LevelEditor>();
    }

    void OnEnable()
    {
        // so that the script can access click events on the scene view
        SceneView.duringSceneGui += OnSceneGUI;
        CreateLevel();

    }

    private void OnDisable()
    {
        // removes the sript access to the scene view
        ToolReset();
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    public void OnInspectorUpdate()
    {
        Repaint();
    }

    /*----------------------
    DONE IN THE SCENE VIEW
    ----------------------*/

    void OnSceneGUI(SceneView sceneView)
    {
        Event eventCurrent = Event.current;

        //cast a ray to check where to place the object in the worldspace
        Ray ray = HandleUtility.GUIPointToWorldRay(eventCurrent.mousePosition);
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(ray, out hitInfo);

        if (previewPrefab != null)
        {
            MeshRenderer renderer = previewPrefab.GetComponent<MeshRenderer>();
            float meshHeight = renderer.bounds.size.y;

            if (hit) // if the prefab was placed on another GameObject
            {
                previewPrefab.transform.position = hitInfo.point;
                previewPrefab.transform.position = new Vector3(Mathf.Round(previewPrefab.transform.position.x), previewPrefab.transform.position.y + meshHeight / 2, Mathf.Round(previewPrefab.transform.position.z));
            }
            else // if the prefab wasn't placed on anything
            {
                float t = -ray.origin.y / ray.direction.y; // to calculate the distance along the ray where it intersects the 0 plane
                previewPrefab.transform.position = ray.origin + t * ray.direction; // this represents the intersection point of the ray and the y plane
                previewPrefab.transform.position = new Vector3(Mathf.Round(previewPrefab.transform.position.x), previewPrefab.transform.position.y + meshHeight / 2, Mathf.Round(previewPrefab.transform.position.z));
            }
        }

        if (selectedPrefab != null)
        {
            if (eventCurrent.type == EventType.MouseDown && eventCurrent.button == 0)
            {

                // instantiate the prefab into the world
                GameObject newObject = PrefabUtility.InstantiatePrefab(selectedPrefab) as GameObject;
                newObject.transform.SetParent(level.transform);
                Transform transform = newObject.transform;

                //records the object so that it can be undone and sets it as dirty (so that unity saves it)
                Undo.RegisterCreatedObjectUndo(newObject, "Place Object");
                EditorUtility.SetDirty(newObject);

                if (hit) // if the prefab was placed on another GameObject
                {
                    transform.position = previewPrefab.transform.position;
                }
                else // if the prefab wasn't placed on anything
                {
                    transform.position = previewPrefab.transform.position;
                }
                Event.current.Use();
            }
        } 
        else if (eraserActive)
        {
            if (eventCurrent.type == EventType.MouseDown && eventCurrent.button == 0)
            {
                if (hitInfo.transform != null)
                {
                    DestroyImmediate(hitInfo.transform.gameObject);
                    Event.current.Use();
                }
            }
        }
    }

    // to preview placement of prefab in the scene view
    void PreviewPrefab()
    {
        // destroys the current previewPrefab if there is one
        previewPrefab = PrefabUtility.InstantiatePrefab(selectedPrefab) as GameObject;
        previewPrefab.layer = 2;
    }


    /*-----------------------------
    DONE IN THE LEVEL EDITOR WINDOW
    -----------------------------*/
    void OnGUI()
    {
        ButtonToolKit(); // Displays the ToolKit Buttons
        GetPrefabFolders(); //gets the prefab folders needed and creates the popup selection
        ListPrefabs(); //

        Event eventCurrent = Event.current;

        // keybind ESCAPE resets the tool state
        if (eventCurrent.keyCode == KeyCode.Escape && eventCurrent.type == EventType.KeyDown)
        {
            ToolReset();

            eventCurrent.Use();
            Debug.Log("Reset");
        }
    }
    // creates a game object named level that holds all the prefabs instantiated
    void CreateLevel()
    {
        if (GameObject.Find("Level") != null) // checks if the scene already has a game object named level
        {
            level = GameObject.Find("Level");
        }
        else
        {
            // creates the level game object
            level = new GameObject();
            level.name = "Level";
            level.AddComponent<MergeMeshesManager>(); // adds the script that merges meshes for optimization
        }
    }

    // in order to prevent the different features from interfering with each other
    void ToolReset()
    {
        eraserActive = false;
        selectedPrefab = null;
        DestroyImmediate(previewPrefab);
        previewPrefab = null;
    }

    void ButtonToolKit()
    {
        GUILayout.BeginHorizontal();

        float buttonWidth = 60f;
        float buttonHeight = 40f;

        // ERASER TOOL
        if (GUILayout.Button("Eraser", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight))) 
        {
            ToolReset(); // prevents tools from interfering
            eraserActive = true;
        }

        GUILayout.EndHorizontal();
    }

    // get the prefab folders and show a dropdown selection in order to choose which folder
    void GetPrefabFolders()
    {
        prefabFolders = AssetDatabase.GetSubFolders(prefabFolderPath);

        for (int i = 0; i < prefabFolders.Length; i++)
        {
            prefabFolders[i] = Path.GetFileName(prefabFolders[i]);

        }

        // upcoming code adds a popup of folders for the player to select from
        string[] folderOptions = new string[prefabFolders.Length]; // will automatically add list of folders within prefab folder eventually

        for (int folder = 0; folder < prefabFolders.Length; folder++)
        {
            folderOptions[folder] = prefabFolders[folder];
        }

        // used to select a folder, nonfunctional at the moment
        selected = EditorGUILayout.Popup("Prefab Folder", selected, folderOptions);
    }
    
    //list the prefabs in image button format
    void ListPrefabs()
    {
        float windowWidth = EditorWindow.GetWindow<LevelEditor>().position.width;

        float buttonSize = 90; // Adjust the size of the buttons as needed
        float buttonsPerRow = Mathf.Floor(windowWidth / (buttonSize + 5));
        int buttonCount = 0;
        Boolean needNewRow = false;

        // Getting a list of all prefabs in a specific folder
        string folderPath = "Assets/Prefabs/" + prefabFolders[selected]; // Change this to the path of your folder
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });

        GUILayout.BeginHorizontal();

        //creates buttons with texture of prefab
        foreach (string prefabGuid in prefabGuids)
        {
            if (buttonCount == buttonsPerRow)
            {
                // gibberish that makes new rows so that it doesn't go out of the editor window
                GUILayout.EndHorizontal();
                GUILayout.BeginVertical();
                GUILayout.EndVertical();
                GUILayout.BeginHorizontal();

                buttonCount = 0;
                needNewRow = true;
            } else if (buttonCount == 1 && needNewRow)
            {
                needNewRow = false; 
            }


            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGuid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            Texture2D prefabImage = AssetPreview.GetAssetPreview(prefab);

            if (prefabImage != null)
            {
                if (GUILayout.Button(prefabImage, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                {
                    ToolReset();
                    selectedPrefab = prefab; // on mouse click input, instantiate the prefab in the world.
                    prefabTexture = prefabImage;
                    PreviewPrefab();
                }
            }
            buttonCount++;
        }

        GUILayout.EndHorizontal();
    }
}