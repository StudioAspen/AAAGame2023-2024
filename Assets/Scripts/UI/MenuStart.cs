using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // This is for entering menus since the player inputs lock and make invisible the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
