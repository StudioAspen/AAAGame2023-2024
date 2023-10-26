using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    /// <summary>
    /// CURRENTLY NONE OF THESE MOVEMENTS EXIST IN THIS BRANCH BUT COMMENTED TO SHOW THE INTENTION
    /// This script controls all the player inputs and calculations for them, like the relative camera position
    /// </summary>

    bool canMove = true;

    //Dash dash;
    //stabanddash stabanddash;
    //slashandslide slashandslide;
    //movement movement;

    // Start is called before the first frame update
    void Start()
    {

        //dash = GetComponent<Dash>();
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
                //dash.Dash();
            }

            // Regular movement
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //movement.Jump();
            }
            Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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
