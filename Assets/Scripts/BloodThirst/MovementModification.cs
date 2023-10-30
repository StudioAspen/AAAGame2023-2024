using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementModification : MonoBehaviour
{
    public float boostForAll; // 0-1 value represent percentage

    public void SetBoost(float boost)
    {
        boostForAll = boost;
    }
}
