using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

///-/////////////////////////////////////////////////////////////////////////////
///
/// Defines the developer console
/// 
public class DeveloperConsole : MonoBehaviour
{
    GameObject player; // Reference for player game object

    ///-/////////////////////////////////////////////////////////////////////////////
    ///
    /// Enum of dev commands
    /// 
    public enum DevCommand
    {
        error,
        help,
        killable,
        kill,
        restart,
        spawn,
        load,
        scene_list
    }
    
    // Variables containing the Console's UI
    GameObject devConsoleUI;
    TextMeshProUGUI consoleLogUI;
    TMP_InputField userInput;

    // Strings containing the contents of the console log and player input
    string input;
    string consoleLogText = "";
    string[] inputModified;

    bool isConsoleOpen = false;

    const string ENEMIES_FOLDER_PATH = "Assets/Prefabs/Enemies"; // the enemy folder path
    string[] enemyNames;

    // TYPE LIST OF SCENES BELOW
    [Tooltip("Enter in scene names below")]
    [SerializeField]
    string[] loadableScenes = new string[]
    {
        "exampleScene",
        "Area3",
        "Area4&5",
        "MichelleLevelScene"
    };

    #region Unity Constructors
    ///-/////////////////////////////////////////////////////////////////////////////
    ///
    private void Start()
    {
        //Get UI's game objects
        devConsoleUI = transform.Find("ScrollView").gameObject;
        devConsoleUI.transform.parent.GetComponent<Canvas>().sortingOrder = 1;
        consoleLogUI = devConsoleUI.transform.Find("Console").Find("Content").Find("Log").GetComponent<TextMeshProUGUI>();
        userInput = devConsoleUI.transform.Find("UserInput").GetComponent<TMP_InputField>();

        // Get Player GameObject in the scene
        player = GameObject.FindGameObjectWithTag("Player");

        Array.Sort(loadableScenes);

        GetEnemyPrefabs();
    }

    ///-/////////////////////////////////////////////////////////////////////////////
    ///
    private void Update()
    {
        CheckForInput();
    }
    #endregion //Unity Constructors

    ///-/////////////////////////////////////////////////////////////////////////////
    ///
    private void GetEnemyPrefabs()
    {
        DirectoryInfo enemyFolder = new DirectoryInfo(ENEMIES_FOLDER_PATH);
        FileInfo[] enemyFiles = enemyFolder.GetFiles();
        enemyNames = new string[enemyFiles.Length];
        for (int i = 0; i < enemyFiles.Length; i++) {
            enemyNames[i] = enemyFiles[i].Name;
        }
    }

    #region Console State
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

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }
    #endregion // Console State

    #region Read Input and Update Console
    ///-/////////////////////////////////////////////////////////////////////////////
    ///
    public void ReadStringInput(string s)
    {
        input = s;
        inputModified = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries); // to split up the command from the parameters
        userInput.text = "";

        // executes the command and focuses the input field
        ExecuteCommand();
        FocusConsole();
    }

    ///-/////////////////////////////////////////////////////////////////////////////
    ///
    private void AddToConsoleLog(string message)
    {
        consoleLogText += "> " + message + "\n";
        consoleLogUI.SetText(consoleLogText);
        UpdateConsoleLog();
    }

    ///-/////////////////////////////////////////////////////////////////////////////
    ///
    public void ClearConsole()
    {
        consoleLogText = "";
        UpdateConsoleLog();
    }

    ///-/////////////////////////////////////////////////////////////////////////////
    ///
    /// Updates the console log to match new text
    /// 
    private void UpdateConsoleLog()
    {
        consoleLogUI.SetText(consoleLogText);
    }
    #endregion // Read Input and Update Console

    ///-/////////////////////////////////////////////////////////////////////////////
    ///
    /// Checks if command inputted exists
    /// 
    bool CheckCommand(string[] command, out DevCommand commandType) // checks if the command inputted exists
    {
        if (command.Length != 0) // makes sure an empty command wasn't sent
        {
            if (Enum.TryParse<DevCommand>(command[0], out commandType)) return true;
            else
            {
                AddToConsoleLog("Error: Command \"" + input + "\" doesn't exist!");
                return false;
            }
        }
        commandType = DevCommand.error;
        return false;
    }

    ///-/////////////////////////////////////////////////////////////////////////////
    ///
    private void ExecuteCommand()
    {
        DevCommand commandType;
        if (CheckCommand(inputModified, out commandType)) // checks if the command exists
        {
            //---------------------
            // EXECUTE THE COMMANDS
            //---------------------
            switch (commandType)
            {
                // HELP COMMAND -> LISTS COMMANDS
                case DevCommand.help:
                    AddToConsoleLog("Documentation: bit.ly/GetMikyledDocs");
                    break;
                // SET PLAYER KILLABLE COMMAND
                case DevCommand.killable:
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
                        AddToConsoleLog("Error: missing true or false"); // tells the user if there is an error
                    }
                    break;
                // KILL PLAYER COMMAND
                case DevCommand.kill:
                    KillPlayer();
                    break;
                // RESTART SCENE COMMAND
                case DevCommand.restart:
                    Restart();
                    break;
                // SPAWN ENEMY COMMAND
                case DevCommand.spawn:
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
                    break;
                // LOAD SCENE COMMAND
                case DevCommand.load:
                    try
                    {
                        Load(inputModified[1]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        AddToConsoleLog("Error: must enter a scene");
                    }
                    catch (Exception)
                    {
                        AddToConsoleLog("Error: Scene does not exist");
                    }
                    break;
                // DISPLAY LIST OF SCENES COMMAND
                case DevCommand.scene_list:
                    DisplaySceneList();
                    break;
            }
        }
    }

    #region command methods
    //-------------------------------------
    // INDIVIDUAL METHODS FOR ALL COMMANDS
    //-------------------------------------

    ///-/////////////////////////////////////////////////////////////////////////////
    ///.
    /// sets the player state to killable or unkillable
    /// 

    void SetKillable(bool state) 
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

    ///-/////////////////////////////////////////////////////////////////////////////
    ///.
    /// Kills the player
    /// 
    void KillPlayer()
    {
        try
        {
            if (player != null)
            {
                Time.timeScale = 1;
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

    ///-/////////////////////////////////////////////////////////////////////////////
    ///.
    /// Reloads the scene
    /// 
    void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    ///-/////////////////////////////////////////////////////////////////////////////
    ///.
    /// Spawns an enemy by prefab name
    /// 
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

        GameObject enemyPrefab = Instantiate(Resources.Load(ENEMIES_FOLDER_PATH + enemyNames[enemyIndex])) as GameObject;
    }

    ///-/////////////////////////////////////////////////////////////////////////////
    ///.
    /// Loads a scene by name
    /// 
    private void Load(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).IsValid())
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            throw new Exception();
        }
    }

    ///-/////////////////////////////////////////////////////////////////////////////
    ///
    /// Display Scene List
    /// 
    private void DisplaySceneList()
    {
        if (loadableScenes.Length == 0)
        {
            AddToConsoleLog("Error: No loadable scenes listed");
        }
        String sceneList = "";

        for (int i = 0; i < loadableScenes.Length; i++)
        {
            sceneList += loadableScenes[i] + ", ";
        }

        sceneList.Remove(sceneList.Length - 2, 2);

        AddToConsoleLog(sceneList);
    }
    #endregion // command methods
}