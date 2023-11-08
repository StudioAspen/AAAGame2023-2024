using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void RestartGame()
    {
        // KarenTestScene would be subbed in for whatever the scene name of the level is

        SceneManager.LoadScene("KarenTestScene");
    }
}
