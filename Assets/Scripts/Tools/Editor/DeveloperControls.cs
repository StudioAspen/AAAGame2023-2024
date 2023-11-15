using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DeveloperControls : EditorWindow
{
    Vector2 scrollPos;
    float windowWidth;
    float windowHeight;

    string userInput = "Enter Command Here";
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

    private void OnGUI()
    {
        windowWidth = EditorWindow.GetWindow<DeveloperControls>().position.width; // gets the window width for sizing gui elements
        windowHeight = EditorWindow.GetWindow<DeveloperControls>().position.height; // gets the window height for sizing gui elements

        // Clear the Console
        if(GUILayout.Button("Clear Console"))
        {
            consoleLog = "";
        }

        userInput = EditorGUILayout.TextField(userInput, GUILayout.Height(20));

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(windowHeight - 20));
            EditorGUILayout.TextArea(consoleLog, GUILayout.ExpandHeight(true));
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

    private void ExecuteCommand()
    {
        consoleLog = "> " + userInput + "\n" + consoleLog;
        userInput = "Enter Command Here";
        scrollPos.y = 0;
    }
}

 