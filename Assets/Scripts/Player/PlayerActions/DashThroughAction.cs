using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashThroughAction : PlayerAction
{
    [SerializeField] float dashSpeed;
    [SerializeField] float boostedDashSpeed;

    Rigidbody rb;
    Collider collider;
    DashMovement dashMovement;
    MovementModification movementModification;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        movementModification = GetComponent<MovementModification>();
        dashMovement = new DashMovement(transform, rb);
        dashMovement.OnDashEnd.AddListener(EndAction);
    }
    
    // Update is called once per frame
    void Update()
    {
        dashMovement.UpdateDashing();
    }

    public void DashThrough(StabableEnviornment stabableEnviornment) {
        collider.isTrigger = true; // Temp implementation for passing through objects
        
        float dashDuration = (stabableEnviornment.dashLength / movementModification.GetBoost(dashSpeed, boostedDashSpeed, true));
        rb.position = stabableEnviornment.dashStartTransform.position;
        dashMovement.Dash(stabableEnviornment.dashLength, dashDuration, stabableEnviornment.dashDir);
    }

    public override void EndAction() {

        OnEndAction.Invoke();
    }
    
}
