using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EyeballGeyser : MonoBehaviour {
    [Header("References")]
    [SerializeField] GameObject bloodGeyser;

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

    private void Update() {
        if (isBlood) {
            if (!isInfinate) {
                geyserTimer -= Time.deltaTime;
                if (geyserTimer <= 0f) {
                    EndBloodGeyser();
                }
            }
        }

        //testing
        if(Input.GetKeyDown(KeyCode.P)) {
            StartBloodGeyser();
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            StartBloodGeyser();
        }
    }
    public void StartBloodGeyser() {
        isBlood = true;
        geyserTimer = duration;

        bloodGeyser.SetActive(true);

        //Setting position and scale
        bloodGeyser.transform.position = transform.position + Vector3.up * (height / 2);
        bloodGeyser.transform.localScale = new Vector3(radius, height / 2, radius);
        bloodGeyser.GetComponent<BloodGeyser>().SetStats(geyserAcceleration, geyserMaxSpeed);
    }
    private void EndBloodGeyser() {
        isBlood = false;

        bloodGeyser.SetActive(false);
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position + Vector3.up * height/2, new Vector3(radius, height, radius));
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * (height - radius));
    }
}
