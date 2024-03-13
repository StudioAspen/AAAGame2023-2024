using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibPlatform : MonoBehaviour
{

    [Header("Variables")]
    [SerializeField] float launchForce; //force applied to player when the ribs close
    [SerializeField] float waitTime; //time the platform waits before closing
    [SerializeField] float timeClosed; //time the platform is closed before reopening
    bool ribsOpen; //whether the ribs r open or not, just for testing right now
    [SerializeField] bool waitingToClose; //set to true as soon as the player lands on the platform for the first time
    [SerializeField] Collider knockbackTrigger;//trigger collider that pushes the player away when the platform closes
    [SerializeField] Collider openCollider;//platform collider when ribs r open
    [SerializeField] Collider closedCollider;//platform collider when ribs r closed (should probably be round so the player cant stand on it)
    [SerializeField] float bloodLostOnHit; //blood lost when hit by knockback volume
    MeshRenderer render; //using to change the platform's color when open/closed

    Timer timerTillClosed = new Timer();
    Timer timerTillOpened = new Timer();

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<MeshRenderer>();
    
    }

    // Update is called once per frame
    void Update()
    {
        timerTillClosed.UpdateTimer();
        timerTillOpened.UpdateTimer();
        if(ribsOpen)
        {
            render.material.color = Color.red;
        }
        else
        {
            render.material.color = Color.green;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.TryGetComponent<PlayerInput>(out PlayerInput input))
        {
            if (!waitingToClose) //if the player hasnt stepped on the platform yet, this starts the timer for when the platform closes
            {
                timerTillClosed.StartTimer(waitTime, CloseRibs);
                waitingToClose = true;
            }
        }
        
    }

    private void CloseRibs()
    {
        timerTillOpened.StartTimer(timeClosed, OpenRibs);
        knockbackTrigger.enabled = true;
        openCollider.enabled = false;
        //play closing animation
        closedCollider.enabled = true;
        ribsOpen = false;
        
    }

    private void OpenRibs()
    {
        closedCollider.enabled = false;
        //play opening animation
        openCollider.enabled = true;
        waitingToClose = false;
        ribsOpen = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        /*if(other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Debug.Log("hit!");
            knockbackTrigger.enabled = false;
        }*/

        if (other.TryGetComponent<PlayerInput>(out PlayerInput playerInput))
        {
            BloodThirst bloodThirst = playerInput.GetComponent<BloodThirst>();
            playerInput.DisableInput();//player input is disabled for an instant so we can apply a knockback force to 'em
            bloodThirst.LoseBlood(bloodLostOnHit); //player loses blood when they get hit by the volume
            playerInput.GetComponentInChildren<PlayerActionManager>().KnockBack(transform.position, launchForce); // Applying knockback
            knockbackTrigger.enabled = false; //should be disabled by the animation instead I think,, once we have it,,,
            playerInput.EnableInput();
        }
    }
}
