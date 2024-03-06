using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBloodOrb : MonoBehaviour
{
    [SerializeField] private float bloodAmount = 5f;

    private MeshRenderer meshRenderer;
    private Collider meshCollider;

    private BloodThirst playerBloodThirst;
    private PlayerKillable playerKillable;

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void Awake()
    {
        playerBloodThirst = GameObject.FindObjectOfType<BloodThirst>();
        playerKillable = GameObject.FindObjectOfType<PlayerKillable>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<Collider>();

        playerKillable.OnDie.AddListener(OnReset);
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playerBloodThirst.GainBlood(bloodAmount, false);
            meshRenderer.enabled = false;
            meshCollider.enabled = false;
        }
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void OnReset()
    {
        meshRenderer.enabled = true;
        meshCollider.enabled = true;
    }
}
