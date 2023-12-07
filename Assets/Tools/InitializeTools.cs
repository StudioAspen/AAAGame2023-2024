using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class InitializeTools
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnSceneAfterLoad()
    {
        Debug.Log("Started");
    }
}
