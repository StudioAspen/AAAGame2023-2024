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

    //variables for downward stab
    bool CanDownwardStab = false;
    float stabButtonTimer = 0.0f;
    public float pressDownTime = 0.5f;
    bool shouldStartTimer = false;
    bool canDownwardStab = true;
    public float downwardStabForce = 700f;
    public Rigidbody rigidbody;
    DownwardStab downwardStabScript;
    private float bufferCheckDistance = 0.1f;
    private float groundedCheckDistance;


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

        //get downwardstab script
        downwardStabScript = GetComponent<DownwardStab>();
        //calculate raycast distance
        groundedCheckDistance = (GetComponent<CapsuleCollider>().height / 2) + bufferCheckDistance;                            

        rigidbody = GetComponent<Rigidbody>();
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
                dash.Dash(direction);
            }

            // Regular movement
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //movement.Jump();
            }
            //movement.Move(direction);
        }

        //check if player is grounded
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -transform.up, out hit, groundedCheckDistance))
        {
            //if grounded can perform downward stab
            CanDownwardStab = false;
        }
        else
        {
            //if not grounded cannot perform downward stab
            CanDownwardStab = true;
        }

        //if allowed to perform downward stab has to check if in the air or not 
        if (CanDownwardStab)
        {
            if (shouldStartTimer)
            {
                //start timer
                stabButtonTimer += Time.deltaTime;
            }
            else if (!shouldStartTimer)
            {
                //reset timer
                stabButtonTimer = 0f;

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                shouldStartTimer = true;
                canDownwardStab = true;

            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                shouldStartTimer = false;

            }
            else if (stabButtonTimer >= pressDownTime && canDownwardStab)
            {
                //perform the downward stab
                downwardStabScript.DownwardStabFunction(downwardStabForce, rigidbody);
                Debug.Log("downward stab initiated");
                canDownwardStab = false;
            }
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
