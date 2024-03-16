using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour {
    [SerializeField] string sceneName;

    public void ChangeScene() {
        SceneManager.LoadScene(sceneName);
    }
}
