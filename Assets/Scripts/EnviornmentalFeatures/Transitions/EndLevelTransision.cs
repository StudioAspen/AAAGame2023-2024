using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelTransision : MonoBehaviour
{
    [SerializeField] string sceneName;
    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent(out PlayerInput input)) {
            SceneManager.LoadScene(sceneName);
        }
    }
}
