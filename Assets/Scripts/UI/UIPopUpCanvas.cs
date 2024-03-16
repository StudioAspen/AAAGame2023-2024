using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class UIPopUpCanvas : MonoBehaviour {    
    CanvasGroup canvasGroup;
    TextMeshProUGUI tmp;
    
    // Start is called before the first frame update
    void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup.alpha = 0;
    }

    public void ShowText(string text) {
        tmp.SetText(text);
        canvasGroup.alpha = 1;
    }

    public void HideText() {
        canvasGroup.alpha = 0;
        tmp.SetText("");
    }
}
