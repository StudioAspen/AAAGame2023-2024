using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DemonSword : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] Transform attackTransform;

    [Range(0f,1f)]
    [SerializeField] float followingSpeed;
    private bool isFollowing = true;

    public UnityEvent<Collider> OnContact = new UnityEvent<Collider>();

    private void Update()
    {
        if (isFollowing)
        {
            //Follwing target transform
            transform.position = Vector3.Lerp(transform.position, followTarget.position, followingSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, followTarget.rotation, followingSpeed);
        }
    }

    //Most likely a TEMPERARY FUNCTION used to put the sword in the right place for attacks before we have animations
    public void AttackPosition()
    {
        isFollowing = false;
        transform.position = attackTransform.position;
        transform.rotation = attackTransform.rotation;
        Invoke("EndAttackPosition", 0.5f);
    }
    public void EndAttackPosition()
    {
        isFollowing = true;
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("colldiing");
        OnContact.Invoke(other);
    }
}
