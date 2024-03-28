using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwordMovement : MonoBehaviour
{
    [Header("Player Positions")]
    [SerializeField] Transform followTarget;
    [SerializeField] Transform stabTransform;
    [SerializeField] Transform slashTransform;
    [SerializeField] Transform downwardStabTransform;
    [SerializeField] Transform dashAttackTransform;

    [Header("Other Variables")]
    [Range(0f,1f)]
    [SerializeField] float followingSpeed;
    [SerializeField] int playerLayerNumber;
    [SerializeField] int notAttackableLayer;

    [Header("Events")]
    public UnityEvent<Collider> OnContact = new UnityEvent<Collider>();
    public UnityEvent OnEndAction = new UnityEvent();

    // Other variables
    private bool isFollowing = true;
    public bool isAttacking = false;
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
    public void StabPosition(float duration)
    {
        currentFollow = stabTransform;
        isAttacking = true;
        Invoke("EndAttackPosition", duration);
    }
    public void SlashPosition(float duration) {
        currentFollow = slashTransform;
        isAttacking = true;
        Invoke("EndAttackPosition", duration);
    }
    public void DownwardAttackPosition() {
        currentFollow = downwardStabTransform;
        isAttacking = true;
    }
    public void DashAttackPosition() {
        currentFollow = dashAttackTransform;
        isAttacking = true;
    }
    public void EndAttackPosition() {
        currentFollow = followTarget;
        isAttacking = false;
        CancelInvoke();
        OnEndAction.Invoke();
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.gameObject.layer.ToString() + " " + playerLayerNumber.ToString());
        if (other.gameObject.layer != gameObject.layer && other.gameObject.layer != notAttackableLayer) {
            OnContact.Invoke(other);
            EndAttackPosition();
        }
    }
}
