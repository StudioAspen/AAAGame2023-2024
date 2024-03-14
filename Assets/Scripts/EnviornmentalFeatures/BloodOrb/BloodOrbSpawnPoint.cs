using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodOrbSpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject bloodOrb; // Just a reference to the prefab
    [SerializeField] float bloodGainAmount; // Amount of blood given to the player
    [SerializeField] float orbScale; // Scaling the size blood orb
    PlayerKillable playerKillable;
    GameObject currentBloodOrb;


    private void Start() {
        playerKillable = FindAnyObjectByType<PlayerKillable>();
        playerKillable.OnDie.AddListener(ResetBloodOrb);

        if(FindOrb(out BloodOrb orb)) {
            currentBloodOrb = orb.gameObject;
            SetOrb(orb);
        }
        else {
            ResetBloodOrb();
        }
    }

    // Resetting blood orb
    private void ResetBloodOrb() {
        if (currentBloodOrb == null) {
            currentBloodOrb = Instantiate(bloodOrb, transform);
            SetOrb(currentBloodOrb.GetComponent<BloodOrb>());
        }
    }

    private void SetOrb(BloodOrb orb) {
        orb.GetComponent<Rigidbody>().useGravity = false;
        orb.gainBloodAmount = bloodGainAmount;
        orb.transform.parent = transform;
        orb.transform.localScale = Vector3.one * orbScale;
    }

    // finding the blood orbs in the children
    private bool FindOrb(out BloodOrb orb) {
        orb = null;
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).TryGetComponent(out BloodOrb foundOrb)) {
                orb = foundOrb;
                return true;
            }
        }
        return false;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, orbScale);
    }
}
