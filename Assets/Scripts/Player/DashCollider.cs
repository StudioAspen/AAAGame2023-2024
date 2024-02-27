using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DashCollider : MonoBehaviour
{
    public UnityEvent<Collider> OnContact = new UnityEvent<Collider>();
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer != gameObject.layer) {
            OnContact.Invoke(other);
        }
    }
}
