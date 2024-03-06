using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickEnemyStabable : MonoBehaviour
{
    [SerializeField] GameObject bloodOrb;
    [SerializeField] int singleBloodAmount;
    [SerializeField] int bloodAmountSpawned;
    [SerializeField] int stunnedBloodAmountSpawned;
    [SerializeField] float stunDuration;
    float stunnedTimer;
    public bool stunned = false;

    Color holder;
    Renderer render;

    private void Start() {
        render = GetComponent<Renderer>();
    }

    private void Update() {
        if(stunned) {
            stunnedTimer += Time.deltaTime;
            if(stunnedTimer >= stunDuration) {
                stunned = false;
                render.material.color = holder;
            }
        }
    }

    public void Stun() {
        if (!stunned) {
            stunnedTimer = 0;
            stunned = true;

            holder = render.material.color;
            render.material.color = Color.red;
        }
    }

    public void Die() {
        if (stunned) {
            for (int i = 0; i < stunnedBloodAmountSpawned; i++) {
                bloodOrb.GetComponent<BloodOrb>().gainBloodAmount = singleBloodAmount;
                Instantiate(bloodOrb, transform.position, Quaternion.identity);
            }
        }
        else {
            for (int i = 0; i < bloodAmountSpawned; i++) {
                bloodOrb.GetComponent<BloodOrb>().gainBloodAmount = singleBloodAmount;
                Instantiate(bloodOrb, transform.position, Quaternion.identity);
            }
        }
        Destroy(gameObject);
    }
}
