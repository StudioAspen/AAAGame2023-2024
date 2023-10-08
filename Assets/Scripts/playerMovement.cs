using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;//adjustable move speed of player
    public float groundDrag;//can be scaled to change how much drag there is on player movement when on ground
    public float jumpHeight;//stength of player jump
    public float jumpCooldown;//how often a player can jump
    public float airMultiplier;//an modifiable air multiplier for jumping
    bool readyToJump;
    public float dashSpeed;//how fast the dash goes
    public float dashSpeedChangeFactor;//how fast the player is after dashing
    private float desiredMoveSpeed;//desired movespeed 
    private float lastDesiredMoveSpeed;//store desired movespeed
    private MovementState lastState;//last known state of the player
    private bool keepMomentum;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;//if space is pressed, jump, space can be changed to suit preference

    [Header("Ground Check")]//checks if player is on the ground, if they are, apply drag
    public float playerHeight;//height of the player model
    public LayerMask whatIsGround;//layer mask that will be assigned to whatever the ground is
    bool grounded;//status of whether player is grounded or not

    public Transform orientation;


    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    private float speedChangeFactor;

    Rigidbody rb;//rigid body of player

    public MovementState state;

    public enum MovementState//whatever movestate the player is in, can add in things like walking, crouching, sprinting, dashing, in the air, etc.
    {
        dashing
    }

    public bool dashing;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb = freezeRotation() = true;
    }

    // Update is called once per frame
    void Update()
    {
        //using the raycast of the player model, we check if a player is grounded or not by taking the bottom half of player height and going a little bit into the ground to make sure
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        playerInput();
        SpeedControl();

        if (grounded && (state != dashing))//if grounded, apply drag, else, have no drag
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    void FixedUpdate()
    {
        MovePlayer();
        StateHandler();
    }

    private void playerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");//input of horizontal camera movement
        verticalInput = Input.GetAxisRaw("Vertical");//input of vertical camera movement

        if(Input.GetKey(jumpKey) && readyToJump && grounded)//if jump key is pressed, the player is ready to jump and the player is on the ground then...
        {
            readyToJump = false;//makes it so player can't jump again to reset jump
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);//allows player to continousally jump as long as jump keybind is being held down
        }
    }

    private void MovePlayer()
    {
        //to be filled in with Joshua's movement code

        if(grouded)//if player is grounded then...
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);//apply force on player movement
        }

        else if (!grounded)//if player is not on the ground then...
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);//apply force and a air multiplier to the jump
        }
    }

    private void StateHandler()
    {
        if (dashing)//if movementstate is dashing then...
        {
            state = MovementState.dashing;//set movement state to dashing
            desiredMoveSpeed = dashSpeed;//change the movespeed to whatever dashspeed is
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;//sees if there has been any changes in desired speed
        if (lastState == MovementState.dashing) keepMomenteum = true;//if the last movement state was dashing, keep the momenteum

        if (desiredMoveSpeedHasChanged)
        {
            if(keepMomentum)
            {
                StopAllCoRoutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;//final desired speed of player
        lastState = state;//getss the last state of player
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)//if player is going faster than movespeed...
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;//calculates what the max velocity should be 
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);//applies those changes to the player to make sure speed is constant
        }
    }

    private IEnumerator SmoothlyLerpMoveSpeed()//implements Mathf.lerp to smooth out momentum 
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while(time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime * boostFactor;
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);//sets the y velocity to 0 to make sure player is not already in the air

        rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
    }

    private ResetJump()
    {
        readyToJump = true;
    }

}
