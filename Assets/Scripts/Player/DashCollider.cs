using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DashCollider : MonoBehaviour
{
    public UnityEvent<GameObject> OnContact = new UnityEvent<GameObject>();
    private void OnTriggerStay(Collider other) {
        //Debug.Log(other.gameObject.layer.ToString() + " " + playerLayerNumber.ToString());
        if (other.gameObject.layer != gameObject.layer) {
            OnContact.Invoke(other.gameObject);
        }
    }
}
