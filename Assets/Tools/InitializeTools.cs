using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InitializeTools
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AddToLoadScene()
    {
        SceneManager.sceneLoaded += OnLoadScene;
    }
    static void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Added Method");
        if (GameObject.Find("DeveloperConsole") == null)
        {
            string devConsolePath = "Tools/DeveloperConsole"; // the enemy folder path
            
            Object.Instantiate(Resources.Load(devConsolePath) as GameObject);
        }
        if (GameObject.Find("EventSystem") == null)
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
    }
}
