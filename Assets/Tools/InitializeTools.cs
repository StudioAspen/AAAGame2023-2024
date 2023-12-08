using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class InitializeTools
{
    static InitializeTools()
    {
        SceneManager.sceneLoaded += OnLoadScene;
    }

    static void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        if (GameObject.Find("DeveloperConsole") == null)
        {
            string UIFolderPath = "Assets/Prefabs/UI"; // the enemy folder path

            string[] UI = AssetDatabase.FindAssets("t:Prefab", new[] { UIFolderPath }); // gets a kust if the prefabs guids

            //gets a list of the enemy prefabs names
            string[] UINames = new string[UI.Length];
            for (int i = 0; i < UI.Length; i++)
            {
                UINames[i] = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(UI[i])).name;
            }

            GameObject devConsolePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(UI[Array.IndexOf(UINames, "DeveloperConsole")]));
            
            PrefabUtility.InstantiatePrefab(devConsolePrefab);
        }
        if (GameObject.Find("EventSystem") == null)
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
    }


}
