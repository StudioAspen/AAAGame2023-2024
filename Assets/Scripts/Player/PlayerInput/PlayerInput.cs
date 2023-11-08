using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    /// <summary>
    /// CURRENTLY NONE OF THESE MOVEMENTS EXIST IN THIS BRANCH BUT COMMENTED TO SHOW THE INTENTION
    /// This script controls all the player inputs and calculations for them, like the relative camera position
    /// 
    /// For the gameplay developers replace the comments with your respective code, 
    /// If there are any design concerns they will be addressed in a code review
    /// </summary>

    bool canMove = true;

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

        //dash.OnStartDash.AddListener(StartingMove);
        //stabAndDash.OnStartStab.AddListener(StartingMove);
        //slashAndSlide.OnStartSlash.AddListener(StartingMove);

        //dash.OnEndDash.AddListener(EndingMove);
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
                //stabAndDash.StartStab();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //slashAndSlide.StartSlash();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
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
    }
}
