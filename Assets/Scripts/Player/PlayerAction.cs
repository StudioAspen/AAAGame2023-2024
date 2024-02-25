using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

abstract public class PlayerAction : MonoBehaviour
{
    public string actionName;
    public UnityEvent OnEndAction = new UnityEvent();
    abstract public void EndAction();
}