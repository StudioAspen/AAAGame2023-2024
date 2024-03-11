using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePadEffect : MonoBehaviour
{
    public float initalSpeedScale; // How much the player impacts the speed, measured in percent (i.e. value of 0.1 == 10% of player speed is factored)
    public float bonusSpeed; // Speed added after the intial speed scale
    public float minimumExitSpeed; // Minimum speed launched BEFORE adding bonusSpeed
}
