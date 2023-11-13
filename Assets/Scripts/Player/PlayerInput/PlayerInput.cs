using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    /// <summary>
    /// CURRENTLY NONE OF THESE MOVEMENTS EXIST IN THIS BRANCH BUT COMMENTED TO SHOW THE INTENTION
    /// This script controls all the player inputs and calculations for them, like the relative camera position
    /// 
    /// For the gameplay developers replace the comments with your respective code, 
    /// If there are any design concerns they will be addressed in a code review
    /// </summary>

    private bool canMove = true;
    private UnityEvent currentMovementEnding;

    DashMovement dash;
    //stabanddash stabanddash;
    //slashandslide slashandslide;
    //movement movement;

    // Start is called before the first frame update
    void Start()
    {

        dash = GetComponent<DashMovement>();
        //stabAndDash = GetComponent<StabAndDash>();
        //slashAndSlide = GetComponent<SlashAndSlide>();
        //movement = GetComponent<Movement>();

        //stabAndDash.OnStartStab.AddListener(StartingMove);
        //slashAndSlide.OnStartSlash.AddListener(StartingMove);

        //stabAndDash.OnEndStab.AddListener(EndingMove);
        //slashAndSlide.OnEndSlash.AddListener(EndingMove);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            //Combat Moves
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("stabbing");
                //stabAndDash.StartStab();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("stabbing");
                //slashAndSlide.StartSlash();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                canMove = false;

                //Setting function for ending dash
                dash.OnDashEnd.AddListener(EndingMove);
                currentMovementEnding = dash.OnDashEnd;
                
                dash.PlayerInputDash(direction);
            }

            // Regular movement
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //movement.Jump();
            }
            //movement.Move(direction);
        }
    }

    public void StartingMove()
    {
        canMove = false;
    }
    public void EndingMove()
    {
        canMove = true;
        currentMovementEnding.RemoveListener(EndingMove);
    }
}
