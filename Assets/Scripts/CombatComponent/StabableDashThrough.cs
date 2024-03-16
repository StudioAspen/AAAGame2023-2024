using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class StabableDashThrough : MonoBehaviour {
    
    public Vector3 dashDir; //direction of the dash
    public float dashLength; //how far the player is launched

    [Header("Adjustable")]
    public bool canGiveBlood;

    abstract public void CalculateDash(GameObject source);
}
