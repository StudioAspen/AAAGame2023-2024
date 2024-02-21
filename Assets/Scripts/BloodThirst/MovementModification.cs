using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementModification : MonoBehaviour
{
    public float boostForAll; // 0-1 value represent percentage
    public UnityEvent OnModifyMovement = new UnityEvent();
    public void SetBoost(float boost)
    {
        boostForAll = boost;
        OnModifyMovement.Invoke();
    }


}

