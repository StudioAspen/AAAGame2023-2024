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
    [MenuItem("Tools/Developer Console")]
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
            if (inputModified[0] == "help") // checks for help command which shows documentation
            {
                AddToConsoleLog("Documentation: https://docs.google.com/document/d/1v9Pv3NSl3cyLKnmF_MCTEz9_mHWrwQd4CHd3luqehxo/edit?usp=sharing");
            }
            else if (inputModified[0] == "killable") // checks for setkillable command
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
            else if (inputModified[0] == "kill") // checks for kill player command
            {
                KillPlayer();
            }
            else if (inputModified[0] == "restart") // checks for restart command
            {
                Restart();
            }
            else if (inputModified[0] == "spawn") // checks for spawn enemy command
            {
                try
                {
                    SpawnEnemy(inputModified[1]); // checks for exceptions, whether it is missing input or invalid enemy names
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

    bool CheckCommand(string[] command) // checks if the command inputted exists
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
        player = GameObject.Find("Player"); // Finds the player in the scene

        if (state) 
        {
            player.GetComponent<PlayerKillable>().isKillable = true; // sets killable to true
            AddToConsoleLog("Player is now killable"); // sends message to the console
        }
        else
        {
            player.GetComponent<PlayerKillable>().isKillable = false; // sets killable to false
            AddToConsoleLog("Player is now unkillable"); // sends message to the console
        }
    }

    void KillPlayer()
    {
        player = GameObject.Find("Player"); // Finds the player in the scene
        player.GetComponent<PlayerKillable>().TakeDamage(player.GetComponent<PlayerKillable>().maxHP);
        AddToConsoleLog("Killed the player");
    }

    // reloads the scene
    void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    // spawns an enemy by the name 
    void SpawnEnemy(string enemy)
    {
        string enemiesFolderPath = "Assets/Prefabs/Enemies"; // the enemy folder path
        
        string[] enemies = AssetDatabase.FindAssets("t:Prefab", new[] { enemiesFolderPath }); // gets a kust if the prefabs guids

        //gets a list of the enemy prefabs names
        string[] enemyNames = new string[enemy.Length];
        for (int i = 0;  i < enemies.Length; i++)
        {
            enemyNames[i] = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(enemies[i])).name;
        }
        int index;
        
        // checks if the enemy exists in the folder
        if (enemyNames.Contains(enemy))
        {
            index = Array.IndexOf(enemyNames, enemy);
        }
        else
        {
            throw new Exception("Error: Enemy does not exist");
        }

        // gets the reference of the enemy for instantiation
        string enemyPrefabPath = AssetDatabase.GUIDToAssetPath(enemies[index]);
        GameObject enemyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(enemyPrefabPath);

        PrefabUtility.InstantiatePrefab(enemyPrefab); // instantiates the enemy
    }

    // adds the message to the console log
    private void AddToConsoleLog(string message)
    {
        consoleLog = "> " + message + "\n" + consoleLog;
    }
}

 