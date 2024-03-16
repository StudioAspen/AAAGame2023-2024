using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GetMikyled.LevelEditor
{
    public partial class LevelEditor
    {
        private const string PREFAB_FOLDER_PATH = "Assets/Prefabs";
        private const string SELECT_FOLDER_NAME = "-- Select Folder --";

        Foldout prefabView;
        VisualElement buttonView;
        DropdownField prefabFolderDropDown;
        List<string> prefabFolderNames;

        GameObject level;
        GameObject selectedPrefab;
        GameObject previewPrefab;

        private void CreateLevel()
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

        #region Construct UI
        ///-//////////////////////////////////////////////////////////////////
        ///
        private void ConstructPrefabPlacerUI(VisualElement root)
        {
            prefabView = root.Q<Foldout>(name: "PrefabView");

            buttonView = new VisualElement();
            buttonView.style.flexDirection = FlexDirection.Row;
            buttonView.style.flexWrap = Wrap.Wrap;
            ConstructPrefabFolderDropdown();
            prefabView.Add(buttonView);
        }

        ///-//////////////////////////////////////////////////////////////////
        ///
        private void ConstructPrefabFolderDropdown()
        {
            GetPrefabFolders();

            // Create dropdown for selecting folder
            prefabFolderDropDown = new DropdownField();
            prefabFolderDropDown.label = "Prefab Folder";
            prefabFolderDropDown.choices = prefabFolderNames;
            prefabFolderDropDown.value = prefabFolderNames[0];
            prefabFolderDropDown.RegisterValueChangedCallback(ReconstructPrefabButtons);
            prefabView.Add(prefabFolderDropDown);
        }

        ///-//////////////////////////////////////////////////////////////////
        ///
        private void ConstructPrefabButtons(string folderName)
        {
            if (folderName != SELECT_FOLDER_NAME)
            {
                buttonView.Clear();

                float buttonSize = 90; // Adjust the size of the buttons as needed

                // Getting a list of all prefabs in a specific folder
                string folderPath = "Assets/Prefabs/" + folderName; // Change this to the path of your folder
                string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });

                foreach (string prefabGuid in prefabGuids)
                {
                    string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGuid);
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                    Texture2D prefabTexture = AssetPreview.GetAssetPreview(prefab);
                    Image prefabImage = new Image();
                    prefabImage.image = prefabTexture;

                    if (prefabImage != null)
                    {
                        Button newPrefabButton = new Button();
                        newPrefabButton.style.width = buttonSize;
                        newPrefabButton.style.height = buttonSize;
                        newPrefabButton.Add(prefabImage);
                        
                        buttonView.Add(newPrefabButton);
                        newPrefabButton.clickable.clicked += () => { SelectPrefab(prefab); };
                    }
                }
            }
        }

        ///-//////////////////////////////////////////////////////////////////
        ///
        private void ReconstructPrefabButtons(ChangeEvent<string> ev)
        {
            ConstructPrefabButtons(ev.newValue);
        }

        ///-//////////////////////////////////////////////////////////////////
        ///
        private void GetPrefabFolders()
        {
            // Get prefab subfolder paths
            string[] prefabFolderPaths = AssetDatabase.GetSubFolders(PREFAB_FOLDER_PATH);

            prefabFolderNames = new List<string>();

            for (int i = 0; i < prefabFolderPaths.Length; i++)
            {
                prefabFolderNames.Add(Path.GetFileName(prefabFolderPaths[i]));

            }

            prefabFolderNames.Insert(0, SELECT_FOLDER_NAME);
        }
        #endregion // Construct UI

        ///-//////////////////////////////////////////////////////////////////
        ///
        private void OnScenePrefabPlacer(SceneView sceneView)
        {
            if (buildMode == BuildMode.PrefabPlacer)
            {

                Event evt = Event.current;
                int id = GUIUtility.GetControlID(FocusType.Passive);

                switch (evt.type)
                {
                    case EventType.Layout:
                    case EventType.MouseMove:
                        {
                            // AddDefaultControl means that if no other control is selected, this will be chosen as the fallback.
                            // This allows things like the translate handle and buttons to function.
                            HandleUtility.AddDefaultControl(id);

                            UpdatePreviewPrefabPosition(evt);

                            break;
                        }
                    case EventType.MouseDown:
                        if (evt.button == 0 && HandleUtility.nearestControl == id)
                        {
                            // Tells the scene view that the placing prefab event is taking place and to ignore other related events 
                            GUIUtility.hotControl = id;

                            PlacePrefab(evt);

                            evt.Use();
                        }

                        break;
                    case EventType.MouseUp:
                        if (evt.button == 0 && GUIUtility.hotControl == id)
                        {
                            GUIUtility.hotControl = 0; // resets hot control

                            evt.Use();
                        }

                        break;
                }
            }
        }

        ///-//////////////////////////////////////////////////////////////////
        ///
        private void PlacePrefab(Event argEvt)
        {
            Transform newPrefab = Instantiate(selectedPrefab).transform;
            newPrefab.parent = level.transform;
            newPrefab.position = previewPrefab.transform.position;  

            //records the object so that it can be undone and sets it as dirty (so that unity saves it)
            Undo.RegisterCreatedObjectUndo(newPrefab.gameObject, "Place Prefab");
            EditorUtility.SetDirty(newPrefab.gameObject);

            argEvt.Use();
        }

        ///-//////////////////////////////////////////////////////////////////
        ///
        private void CreatePreviewPrefab()
        {
            previewPrefab = PrefabUtility.InstantiatePrefab(selectedPrefab) as GameObject;
            previewPrefab.transform.SetParent(mainToolObject);
            previewPrefab.layer = 2;
        }

        ///-//////////////////////////////////////////////////////////////////
        ///
        private void UpdatePreviewPrefabPosition(Event argEvt)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(argEvt.mousePosition);
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(ray, out hitInfo);

            if (previewPrefab != null)
            {
                MeshRenderer renderer = previewPrefab.GetComponent<MeshRenderer>();
                float meshHeight = renderer.bounds.size.y;

                if (hit) // if the prefab was placed on another GameObject
                {
                    previewPrefab.transform.position = hitInfo.point;
                }
                else // if the prefab wasn't placed on anything
                {
                    float t = -ray.origin.y / ray.direction.y; // to calculate the distance along the ray where it intersects the 0 plane
                    previewPrefab.transform.position = ray.origin + t * ray.direction; // this represents the intersection point of the ray and the y plane
                }

                if (doGridSnapping)
                {
                    previewPrefab.transform.position = new Vector3(previewPrefab.transform.position.x.Round(gridSize), previewPrefab.transform.position.y + meshHeight / 2, previewPrefab.transform.position.z.Round(gridSize));
                }
                else
                {
                    previewPrefab.transform.position = new Vector3(previewPrefab.transform.position.x, previewPrefab.transform.position.y + meshHeight / 2, previewPrefab.transform.position.z);
                }
            }
        }

        ///-//////////////////////////////////////////////////////////////////
        ///
        private void SelectPrefab(GameObject argPrefab)
        {
            SetBuildMode(BuildMode.PrefabPlacer);
            selectedPrefab = argPrefab;

            CreatePreviewPrefab();
        }
    }
}
