using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DeveloperControls : EditorWindow
{

    float windowWidth;

    [MenuItem("Tools/Developer Controls")]
   public static void ShowWindow()
    {
        EditorWindow.GetWindow<DeveloperControls>();
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }

    private void OnGUI()
    {
        windowWidth = EditorWindow.GetWindow<DeveloperControls>().position.width;

        string userInput = "Enter Command Here";
        userInput = EditorGUI.TextField(new Rect(0, 0, windowWidth, 20), userInput);

        Event eventCurrent = Event.current;
        if (eventCurrent.type == EventType.KeyDown) {
            if (eventCurrent.keyCode == KeyCode.Return)
            {
                GUI.Label(new Rect(0, 30, windowWidth, 20), userInput);
            }
        }
    }

    void AddLine(string text)
    {
        Debug.Log("Printed");
        GUI.Label(new Rect(10,10,windowWidth, 20), text);
    }
}

