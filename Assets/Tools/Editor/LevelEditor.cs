using UnityEngine;
using UnityEditor;
using System.Drawing.Printing;

public class LevelEditor : EditorWindow
{

    Vector2 scrollPosition;
    float prefabAreaWidth;
    float prefabAreaHeight;

    GameObject selectedPrefab;


    [MenuItem("Tools/Level Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<LevelEditor>();
    }

    void OnGUI()
    {
        // BEGIN PREFAB AREA
        prefabAreaWidth = GetWindow<LevelEditor>().position.width - 10;
        prefabAreaHeight = GetWindow<LevelEditor>().position.height / 2;

        GUILayout.BeginArea(new Rect(10, 10, prefabAreaWidth, prefabAreaHeight));

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        int selected = 0; 
        string[] folderOptions = new string[] // will automatically add list of folders within prefab folder eventually
        {
            "option 1", "option 2", "option 3"
        };

        // used to select a folder, nonfunctional at the moment
        selected = EditorGUILayout.Popup("Prefab Folder", selected, folderOptions);

        int buttonSize = 80; // Adjust the size of the buttons as needed

        // Getting a list of all prefabs in a specific folder
        string folderPath = "Assets/Prefabs"; // Change this to the path of your folder
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });

        GUILayout.BeginHorizontal();
        //creates buttons with texture of prefab
        foreach (string prefabGuid in prefabGuids)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGuid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            Texture2D prefabImage = AssetPreview.GetAssetPreview(prefab);

            if (prefabImage != null)
            {
                if (GUILayout.Button(prefabImage, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                {
                    selectedPrefab = prefab; // on mouse click input, instantiate the prefab in the world.
                }
            }
        }


        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();

        GUILayout.EndArea();
    }
}