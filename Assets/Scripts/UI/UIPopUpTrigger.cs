using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class UIPopUpTrigger : MonoBehaviour {
    
    [SerializeField, TextArea] string popUpContent;
    
    UIPopUpCanvas uiPopUpCanvas;

    void Start() {
        uiPopUpCanvas = FindFirstObjectByType<UIPopUpCanvas>();

        if (uiPopUpCanvas == null) {
            Debug.LogError("[UIPopUpTrigger] Could not find UIPopUpCanvas. Is the prefab in the scene?");
        }
    }

    void OnTriggerEnter() {
        uiPopUpCanvas.ShowText(popUpContent);
    }

    void OnTriggerExit() {
        uiPopUpCanvas.HideText();
    }

    void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "icon_speech_balloon.png", true);
    }
}
