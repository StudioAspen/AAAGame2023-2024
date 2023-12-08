using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class DeveloperConsole : MonoBehaviour
{
    GameObject player; // Reference for player game object

    // an array that contains all the possible commands
    string[] commandArray = new string[]
    {
        "help", "killable", "kill", "restart", "spawn"
    };
    
    // Variables containing the Console's UI
    GameObject devConsoleUI;
    TextMeshProUGUI consoleLogUI;
    TMP_InputField userInput;

    // Strings containing the contents of the console log and player input
    string input;
    string consoleLogText = "";
    string[] inputModified;

    bool isConsoleOpen = false;

    string enemiesFolderPath;
    string[] enemies;
    string[] enemyNames;


    private void Start()
    {
        //Get UI's game objects
        devConsoleUI = transform.Find("ScrollView").gameObject;
        consoleLogUI = devConsoleUI.transform.Find("Console").Find("Content").Find("Log").GetComponent<TextMeshProUGUI>();
        userInput = devConsoleUI.transform.Find("UserInput").GetComponent<TMP_InputField>();

        // Get Player GameObject in the scene
        player = GameObject.FindGameObjectWithTag("Player");


        ///////////////////////////////////////////////////////////////
        /// Gets all the references necessary to spawn enemy prefabs///
        ///////////////////////////////////////////////////////////////
        enemiesFolderPath = "Assets/Prefabs/Enemies"; // the enemy folder path

        enemies = AssetDatabase.FindAssets("t:Prefab", new[] { enemiesFolderPath }); // gets an array if the prefabs guids

        //gets a list of the enemy prefabs names
        enemyNames = new string[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyNames[i] = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(enemies[i])).name;
        }
    }

    private void Update()
    {
        CheckForInput();
    }

    // Focuses the console so that you are already typing into the input field
    private void FocusConsole()
    {
        userInput.Select();
        userInput.ActivateInputField();
    }

    /// Checks for input from the player
    private void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote)) // checks if tilde was pressed
        {
            isConsoleOpen = !isConsoleOpen;
            DevConsoleStatus(isConsoleOpen);
        }
    }

    /// <summary>
    /// Checks if the dev console is open or not, then opens or closes is it accordingly
    /// </summary>
    private void DevConsoleStatus(bool newStatus)
    {
        devConsoleUI.SetActive(newStatus); // opens or closes the dev console
        if (newStatus) // if it opens the console, focus the console and clear it
        {
            FocusConsole();
            ClearConsole();
        }
    }

    // Reads input from input field
    public void ReadStringInput(string s)
    {
        input = s;
        inputModified = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries); // to split up the command from the parameters
        userInput.text = "";

        // executes the command and focuses the input field
        ExecuteCommand();
        FocusConsole();
    }

    // adds the message to the console log
    private void AddToConsoleLog(string message)
    {
        consoleLogText += "> " + message + "\n";
        consoleLogUI.SetText(consoleLogText);
        UpdateConsoleLog();
    }

    // clears the console
    public void ClearConsole()
    {
        consoleLogText = "";
        UpdateConsoleLog();
    }

    // updates the console log to match the new text
    private void UpdateConsoleLog()
    {
        consoleLogUI.SetText(consoleLogText);
    }

    // checks if the command inputted exists
    bool CheckCommand(string[] command) // checks if the command inputted exists
    {
        if (command.Length != 0) // makes sure an empty command wasn't sent
        {
            if (commandArray.Contains<string>(command[0])) return true;
            else
            {
                AddToConsoleLog("Error: Command \"" + input + "\" doesn't exist!");
                return false;
            }
        }
        return false;
    }

    // checks if a command exists and executes it if it does
    private void ExecuteCommand()
    {
        if (CheckCommand(inputModified)) // checks if the command exists
        {
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
                    }
                    else
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

    //-------------------------------------
    // INDIVIDUAL METHODS FOR ALL COMMANDS
    //-------------------------------------

    void SetKillable(bool state) // sets the player state to killable or unkillable
    {
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
        try
        {
            if (player != null)
            {
                player.GetComponent<PlayerKillable>().TakeDamage(player.GetComponent<PlayerKillable>().maxHP);
                AddToConsoleLog("Killed the player");
            }
            else
            {
                throw new Exception();
            }
        }
        catch (Exception)
        {
            AddToConsoleLog("Error: player cannot be found");
        }
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
        int enemyIndex;

        // checks if the enemy exists in the folder
        if (enemyNames.Contains(enemy))
        {
            enemyIndex = Array.IndexOf(enemyNames, enemy);
        }
        else
        {
            throw new Exception("Error: Enemy does not exist");
        }

        // gets the reference of the enemy for instantiation
        string enemyPrefabPath = AssetDatabase.GUIDToAssetPath(enemies[enemyIndex]);
        GameObject enemyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(enemyPrefabPath);

        Instantiate(enemyPrefab); // instantiates the enemy
    }
}