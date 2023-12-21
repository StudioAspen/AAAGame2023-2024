using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwordMovementTemp : SwordAnimation
{
    [Header("Player Positions")]
    [SerializeField] Transform followTarget;
    [SerializeField] Transform attackTransform;
    [SerializeField] Transform downwardStabTransform;
    [SerializeField] Transform dashAttackTransform;

    [Header("Other Variables")]
    [Range(0f,1f)]
    [SerializeField] float followingSpeed;
    [SerializeField] int playerLayerNumber;
    [SerializeField] float attackDuration;
    // Other variables
    private bool isFollowing = true;
    Transform currentFollow;

    private void Start() {
        currentFollow = followTarget;
    }

    private void Update()
    {
        if (isFollowing)
        {
            //Follwing target transform
            transform.position = Vector3.Lerp(transform.position, currentFollow.position, followingSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, currentFollow.rotation, followingSpeed);
        }

    }

    //Most likely a TEMPERARY FUNCTION used to put the sword in the right place for attacks before we have animations
    public void AttackPosition(float duration)
    {
        currentFollow = attackTransform;
        Invoke("EndAttackPosition", duration);
    }
    public override void StartStabAnimation() {
        AttackPosition(attackDuration);
    }
    public override void StartSlashAnimation() {
        AttackPosition(attackDuration);
    }
    public override void StartDownwardStabAnimation() {
        currentFollow = downwardStabTransform;
    }
    public override void StartSlashDashAnimation() {
        currentFollow = dashAttackTransform;
    }
    public override void StartStabDashAnimation() {
        currentFollow = dashAttackTransform;
    }
    public override void EndAnimation() {
        EndAttackPosition();
    }

    public void EndAttackPosition() {
        currentFollow = followTarget;
        CancelInvoke();
        OnEndAnimation.Invoke();
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.gameObject.layer.ToString() + " " + playerLayerNumber.ToString());
        if (other.gameObject.layer != gameObject.layer) {
            OnContact.Invoke(other.gameObject);
            EndAttackPosition();
        }
    }
}
