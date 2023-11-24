using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerStay(Collider other) {
        if(other.TryGetComponent(out Killable killable)) {
            killable.TakeDamage(100000);
        }
    }
}
