using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementModification : MonoBehaviour
{
    public float boostForAll; // 0-1 value represent percentage

<<<<<<< Updated upstream
=======
    public PlayerMovement speed;
    public float test = 0f; //placeholder for blood gauge
    public bool boostOn = true;

 
       
>>>>>>> Stashed changes
    public void SetBoost(float boost)
    {
        boostForAll = boost;
    }
<<<<<<< Updated upstream
}
=======

    void Update()
    {

        if(test >= 5 && boostOn) //whenever blood gauge reaches over the activation theshold
        {
          boostOn = false;
            speed.moveSpeed += speed.moveSpeed * boostForAll;
        }

        if(test < 5 && boostOn == false) ///whenever blood gauge reaches under the activation theshold
        {
            speed.moveSpeed = 5;
            boostOn = true;
        }
    }
   
}


>>>>>>> Stashed changes
