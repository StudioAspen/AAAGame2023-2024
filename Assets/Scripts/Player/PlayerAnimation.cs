using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    DashMovement dashMovement;
    Animator anim;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();; 
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
            if (dashMovement.isDashing)
            {
                anim.SetBool("isDashing", true);
            }
            else
            {
                anim.SetBool("isDashing", false);
                anim.SetBool("isWalking", true);
            }
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
}
