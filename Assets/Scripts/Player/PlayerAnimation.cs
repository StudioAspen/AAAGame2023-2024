using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    DashAction dashAction;

    Animator anim;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
        dashAction = GetComponentInParent<DashAction>();

        dashAction.OnStartAction.AddListener(DashAnimation);
        dashAction.OnEndAction.AddListener(EndDashAnimation);
    }

    // Update is called once per frame
    void Update()
    {
        Animation();
    }

    void Animation()
    {

        if (rb.velocity.x != 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        if (rb.velocity.y != 0)
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }
    }

    public void DashAnimation()
    {
        anim.SetBool("isDashing", true);
    }

    public void EndDashAnimation()
    {
        anim.SetBool("isDashing", false);
    }
}
