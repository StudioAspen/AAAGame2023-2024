using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeveloperControls : EditorWindow
{
    // an array that contains all the possible commands
    string[] commandArray = new string[]
    {
        "help", "killable", "kill", "restart", "spawn"
    };

    // stuff that the commands modify
    GameObject player;

    // for formatting the gui
    Vector2 scrollPos;
    float windowWidth;
    float windowHeight;

    // deals with taking in user input and the console log
    string userInput = "Enter Command Here";
    string[] inputModified;
    string consoleLog = "";

    // Open the Editor Window
    [MenuItem("Tools/Developer Controls")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<DeveloperControls>();
    }

    private void OnInspectorUpdate() // To repaint the window
    {
        Repaint();
    }

    private void OnEnable()
    {
        player = GameObject.Find("Player"); // finds the player's game object for modification
    }

    private void OnGUI()
    {
        windowWidth = EditorWindow.GetWindow<DeveloperControls>().position.width; // gets the window width for sizing gui elements
        windowHeight = EditorWindow.GetWindow<DeveloperControls>().position.height; // gets the window height for sizing gui elements

        // Clear the Console
        if(GUILayout.Button("Clear Console"))
        {
            consoleLog = "";
        }

        userInput = EditorGUILayout.TextField(userInput, GUILayout.Height(20)); // gets commands from user input
        inputModified = userInput.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries); // to split up the command from the parameters

        //formats the console log into a vertical scroll view
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(windowHeight - 20));
            EditorGUILayout.TextArea(consoleLog, GUILayout.ExpandHeight(true)); // creates the console log
        EditorGUILayout.EndScrollView();

        // Detects whether or not the enter key was pressed (Checks if a command was entered)
        Event eventCurrent = Event.current;
        if (eventCurrent.type == EventType.KeyDown)
        {
            if (eventCurrent.keyCode == KeyCode.Return)
            {
                ExecuteCommand();
            }
        }
    }

    // checks if a command exists and executes it if it does
    private void ExecuteCommand()
    {
        if (CheckCommand(inputModified)) // checks if the command exists
        {
            // resets the command input to default text and resets scroll position
            userInput = "Enter Command Here";
            scrollPos.y = 0;

            //---------------------
            // EXECUTE THE COMMANDS
            //---------------------
            // checks if it is the killable command
            if (inputModified[0] == "killable") // checks for setkillable command
            {
                try // to catch missing or incorrect parameters or values
                {
                    if (inputModified[1] == "true")
                    {
                        SetKillable(true); // sets killable to true
                    }
                    else if (inputModified[1] == "false")
                    {
                        SetKillable(false); // sets killable to false
                    } else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    AddToConsoleLog("Error: missing true or false"); // tells the user if there is an error
                }
            }
            else if (inputModified[0] == "restart") // checks for restart command
            {
                Restart();
            }
            else if (inputModified[0] == "spawn") // checks for spawn enemy command
            {
                try
                {
                    SpawnEnemy(inputModified[1]);
                }
                catch (IndexOutOfRangeException)
                {
                    AddToConsoleLog("Error: must enter an enemy");
                }
                catch (Exception)
                {
                    AddToConsoleLog("Error: enemy does not exist");
                }
            }

        }
    }

    bool CheckCommand(string[] command)
    {
        if (commandArray.Contains<string>(command[0]))
        {
            return true;
        }
        else
        {
            AddToConsoleLog("Error: Command \"" + userInput + "\" doesn't exist!");
            return false;
        }
    }

    //-------------------------------------
    // INDIVIDUAL METHODS FOR ALL COMMANDS
    //-------------------------------------

    void SetKillable(bool state) // sets the player state to killable or unkillable
    {
        if (state)
        {
            AddToConsoleLog("Player is now killable");
        }
        else
        {
            AddToConsoleLog("Player is now unkillable");
        }
    }

    void Kill()
    {
        AddToConsoleLog("Killed the player");
    }

    void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    void SpawnEnemy(string enemy)
    {
        string enemiesPath = Application.dataPath + "/Prefabs/Enemies";
        string[] enemies = AssetDatabase.FindAssets("t:Prefab", new[] { enemiesPath });
        int index;
        if (enemies.Contains(enemy))
        {
            index = Array.IndexOf(enemies, enemy);
        }
        else
        {
            throw new Exception("Error: Enemy does not exist");
        }

        string prefabPath = AssetDatabase.GUIDToAssetPath(enemies[index]);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        PrefabUtility.InstantiatePrefab(prefab);
    }

    private void AddToConsoleLog(string message)
    {
        consoleLog = "> " + message + "\n" + consoleLog;
    }
}

 