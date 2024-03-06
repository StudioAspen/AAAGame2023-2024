using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBloodOrb : MonoBehaviour
{
    [SerializeField] private float bloodAmount = 5f;

    [Space]
    [SerializeField] private GameObject vacuumTrigger;

    private bool canMove = true;

    private Vector3 resetPosition;

    private MeshRenderer meshRenderer;
    private Collider meshCollider;

    private BloodThirst playerBloodThirst;
    private PlayerKillable playerKillable;

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void Awake()
    {
        // Get Player Scripts
        playerBloodThirst = GameObject.FindObjectOfType<BloodThirst>();
        playerKillable = GameObject.FindObjectOfType<PlayerKillable>();

        // Get Floating Blood Orb Components
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<Collider>();

        // Subscribe Reset Method and Data
        playerKillable.OnDie.AddListener(OnReset);
        GetResetData();
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void OnCollisionEnter(Collision collision)
    {
        // If triggered by player, disable floating blood orb and give player blood
        if (collision.transform.CompareTag("Player"))
        {
            playerBloodThirst.GainBlood(bloodAmount, false);
            meshRenderer.enabled = false;
            meshCollider.enabled = false;
            vacuumTrigger.SetActive(false);
        }
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void GetResetData()
    {
        resetPosition = transform.position;
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void OnReset()
    {
        meshRenderer.enabled = true;
        meshCollider.enabled = true;
        vacuumTrigger.SetActive(true);

        transform.position = resetPosition;
    }
}
