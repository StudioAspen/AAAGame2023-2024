using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class StabableDashThrough : MonoBehaviour {
    [Header("Adjustable")]
    public bool canGiveBlood;
    
    public Vector3 dashDir; //direction of the dash
    public float dashLength; //how far the player is launched

    abstract public void CalculateDash(GameObject source);
}
