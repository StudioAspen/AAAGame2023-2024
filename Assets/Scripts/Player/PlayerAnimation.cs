using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // references
    BasicMovementAction groundedCheck;
    DashAction dashAction;
    SlashAction slashAction;
    StabAction stabAction;

    // components
    Animator anim;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // getting components
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
        dashAction = GetComponentInParent<DashAction>();
        slashAction = GetComponentInParent<SlashAction>();
        stabAction = GetComponentInParent<StabAction>();

        dashAction.OnStartAction.AddListener(DashAnimation);
        dashAction.OnEndAction.AddListener(EndDashAnimation);
    }

    // Update is called once per frame
    void Update()
    {
        BasicAnimation();
        AttackAnimation();
    }

    void BasicAnimation()
    {
        // walking animation works based on pos/neg x velocity
        if (rb.velocity.x != 0) {
            anim.SetBool("isWalking", true);
        }
        else {
            anim.SetBool("isWalking", false);
        }

        // jumping animation works based on pos/neg y velocity
        if (rb.velocity.y != 0) {
            anim.SetBool("isJumping", true);
        }
        else {
            anim.SetBool("isJumping", false);
        }
    }

    public void DashAnimation() {
        anim.SetBool("isDashing", true);
    }

    public void EndDashAnimation() {
        anim.SetBool("isDashing", false);
    }

    void AttackAnimation()
    {
        // test
        if (slashAction.isSlashing == true)
        {
            anim.SetBool("isAttacking", true);
            anim.SetTrigger("SlashAttack");

            Debug.Log("Player is Slashing.");
        }
        else if (stabAction.isStabbing == true)
        {
            anim.SetBool("isAttacking", true);
            anim.SetTrigger("StabAttack");

            Debug.Log("Player is Stabbing.");
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
    }
}
