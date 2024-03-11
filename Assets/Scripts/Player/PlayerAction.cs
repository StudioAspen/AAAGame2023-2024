using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

abstract public class PlayerAction : MonoBehaviour
{
    public UnityEvent OnEndAction = new UnityEvent();
    public UnityEvent OnStartAction = new UnityEvent();
    abstract public void EndAction();
}