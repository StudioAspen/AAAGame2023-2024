using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibPlatform : MonoBehaviour
{

    [Header("Variables")]
    [SerializeField] float launchForce;
    [SerializeField] float waitTime;
    [SerializeField] float timeClosed;
    [SerializeField] bool ribsOpen;
    [SerializeField] bool waitingToClose;
    [SerializeField] Collider knockbackTrigger;
    [SerializeField] Collider openCollider;
    [SerializeField] Collider closedCollider;

    Timer timer = new Timer();

    // Start is called before the first frame update
    void Start()
    {
        if (ribsOpen)
        {
            OpenRibs();
        }
        else
        {
            CloseRibs();
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        timer.UpdateTimer();
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (ribsOpen && !waitingToClose)
        {
            Debug.Log("timer started");
            timer.StartTimer(waitTime, CloseRibs);
            waitingToClose = true;
        }
    }

    private void CloseRibs()
    {
        Debug.Log("closing");
        knockbackTrigger.enabled = true;
        openCollider.enabled = false;
        //play closing animation
        knockbackTrigger.enabled = false;
        closedCollider.enabled = true;
        ribsOpen = false;
        timer.StartTimer(timeClosed, OpenRibs);
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
        if(other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Debug.Log("hit!");
            rb.AddForce(Vector3.Normalize(-rb.transform.forward + transform.up) * launchForce, ForceMode.Impulse);
        }
    }
}
