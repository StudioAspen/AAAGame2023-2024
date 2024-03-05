using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EyeballGeyser : DownwardStabEffect {
    [Header("References")]
    [SerializeField] GameObject bloodGeyserPrefab;

    [Header("Geyser Size")]
    [SerializeField] float height;
    [SerializeField] float radius;
    [SerializeField] float duration;
    [SerializeField] bool isInfinate;

    [Header("Geyser Functionality")]
    [SerializeField] float geyserAcceleration;
    [SerializeField] float geyserMaxSpeed;


    // Variables
    float geyserTimer;
    bool isBlood = false;
    GameObject currentBloodGeyser;

    private void Update() {
        if (isBlood) {
            if (!isInfinate) {
                geyserTimer -= Time.deltaTime;
                if (geyserTimer <= 0f) {
                    EndBloodGeyser();
                }
            }
        }
    }

    override public void TriggerEffect() {
        StartBloodGeyser();
    }

    public void StartBloodGeyser() {
        if (!isBlood) {
            isBlood = true;
            geyserTimer = duration;


            currentBloodGeyser = Instantiate(bloodGeyserPrefab, transform.position, Quaternion.identity);

            //Setting position and scale
            currentBloodGeyser.transform.position = transform.position + Vector3.up * (height / 2);
            currentBloodGeyser.transform.localScale = new Vector3(radius, height / 2, radius);
            currentBloodGeyser.GetComponent<BloodGeyser>().SetStats(geyserAcceleration, geyserMaxSpeed);
        }
    }
    private void EndBloodGeyser() {
        if (isBlood) {
            isBlood = false;

            Destroy(currentBloodGeyser);
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position + Vector3.up * height/2, new Vector3(radius, height, radius));
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * (height - radius));
    }
}
