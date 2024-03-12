using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodOrb : VacuumableObject
{
    [Header("Blood Orb")]
    [SerializeField] public float gainBloodAmount;

    private bool consumed = false;
    private Vector3 resetPosition;

    private MeshRenderer meshRenderer;
    private Collider meshCollider;

    private PlayerKillable playerKillable;

    ///-//////////////////////////////////////////////////////////////////////
    ///
    protected override void Start()
    {
        playerKillable = FindObjectOfType<PlayerKillable>();
        player = playerKillable.transform;

        meshCollider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();

        GetResetData();

        playerKillable.OnDie.AddListener(OnReset);
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BloodThirst bloodThirst))
        {
            bloodThirst.GainBlood(gainBloodAmount, true);

            meshRenderer.enabled = false;
            meshCollider.enabled = false;

            consumed = true;
        }
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    protected override void InVacuumUpdate()
    {
        if (consumed == false)
        {
            base.InVacuumUpdate();
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

        consumed = false;

        transform.position = resetPosition;
    }
}