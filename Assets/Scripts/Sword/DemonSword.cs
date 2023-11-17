using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DemonSword : MonoBehaviour
{
    [Header("Player Positions")]
    [SerializeField] Transform followTarget;
    [SerializeField] Transform attackTransform;
    Transform currentFollow;

    [Header("Other Variables")]
    [Range(0f,1f)]
    [SerializeField] float followingSpeed;
    [SerializeField] int playerLayerNumber;

    [Header("Events")]
    public UnityEvent<Collider> OnContact = new UnityEvent<Collider>();
    public UnityEvent OnEndAction = new UnityEvent();

    // Other variables
    private bool isFollowing = true;

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
    public void AttackPosition()
    {
        currentFollow = attackTransform;
        Invoke("EndAttackPosition", 0.5f);
    }
    public void EndAttackPosition() {
        currentFollow = followTarget;
        OnEndAction.Invoke();
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.gameObject.layer.ToString() + " " + playerLayerNumber.ToString());
        if (other.gameObject.layer != playerLayerNumber) {
            OnContact.Invoke(other);
            EndAttackPosition();
        }
    }
}
