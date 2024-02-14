using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GetMikyled.LevelEditor
{
    public partial class LevelEditor
    {
        ///-//////////////////////////////////////////////////////////////////
        ///
        private void ConstructEraserUI()
        {
            Button eraserButton = rootVisualElement.Q<Button>(name: "EraserButton");
            eraserButton.clicked += () => EnableEraserTool();
        }

        ///-//////////////////////////////////////////////////////////////////
        ///
        private void OnSceneEraser(SceneView sceneView)
        {
            if (buildMode == BuildMode.Eraser)
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

                            break;
                        }
                    case EventType.MouseDown:
                        if (evt.button == 0 && HandleUtility.nearestControl == id)
                        {
                            // Tells the scene view that the placing prefab event is taking place and to ignore other related events 
                            GUIUtility.hotControl = id;

                            TryEraseHitObject(evt);
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
        private void EnableEraserTool()
        {
            SetBuildMode(BuildMode.Eraser);
        }

        ///-//////////////////////////////////////////////////////////////////
        ///
        private void TryEraseHitObject(Event argEvt)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(argEvt.mousePosition);
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(ray, out hitInfo);

            Debug.Log(hit);

            if(hitInfo.transform != null)
            {
                Debug.Log("Destroyed");
                DestroyImmediate(hitInfo.transform.gameObject);
                argEvt.Use();
            }
        }
    }
}
