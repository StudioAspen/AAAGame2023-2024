using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class dash : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;//reference to orientation of player
    public Transform playerCam;//reference to orientation of playerCam
    private Rigidbody rb;//rigid body of player
    private playerMovement pm;//player movement for dashing

    [Header("References")]
    public float dashDistance;//How far the dash will go
    public float dashUpwardForce;//Force of the dash
    public float dashDuration;//How long the dash lasts

    [Header("Cooldown")]
    public float dashCooldown;//Cooldown for the dash
    private float dashCdTimer;//Time before you can dash again

    [Header("Keybind")]
    public KeyCode dashKey = KeyCode.E;//dash keybind, E can be changed to preference



    private Vector3 delayedForceToApply;//used to apply a delayed force to dash to allow smooth player movement

    public UnityEvent OnDashStart;
    public UnityEvent OnDashEnd;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(dashKey))
        {
            Dash();
        }
        if(dashCdTimer > 0)//if dash is still on cd, count down the timer
        {
            dashCdTimer -= Time.deltaTime;
        }
    }

    private void Dash()
    {
        if (dashCdTimer > 0)//if dash is still on cooldown, return
            return;
        else//else, dash and then start timer
            dashCdTimer = dashCooldown;
        pm.dashing = true;

        Vector3 forceToApply = orientation.forward * dashDistance + orientation.up * dashUpwardForce;
        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);
        rb.AddForce(forceToApply, ForceMode.Impulse);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void DelayedDashForce()//makes sure dash is smooth and does not delay movement
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashing = false;
    }
}
