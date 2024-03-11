using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal.Internal;

public class EnergyBlast : MonoBehaviour
{
    [System.NonSerialized]
    public UnityEvent OnChargeTimer = new UnityEvent();

    [Header("References")]
    public GameObject energyBlast;
    public RectTransform reticle;
    public LayerMask ignoredLayers;
    BloodThirst bloodThirst;

    [Header("Variables")]
    public float maxRange; // How far you can shoot
    private float rechargeTimer; // How long it takes to gain another charge
    public float timeBetweenShots; // Time it takes to fire another shot after just shooting one\
    public float bloodPerShot; //Amount of used per energy blast.

    [System.NonSerialized]

    public float currRechargeTimer = 0f;
    private float currTimeBetweenShots;


    private void Start()
    {
        currTimeBetweenShots = timeBetweenShots;
    }

    private void Update()
    {
        Recharge();

        if (currTimeBetweenShots < timeBetweenShots)
        {
            currTimeBetweenShots += Time.deltaTime;
        }
    }

    public void Shoot()
    {
        if (currTimeBetweenShots >= timeBetweenShots)
        {
            ShootInternal();
            currTimeBetweenShots = 0f;
            GetComponent<BloodThirst>().LoseBlood(bloodPerShot);

        }
    }

    private void ShootInternal()
    {
        Ray ray = Camera.main.ScreenPointToRay(reticle.position);
        float dist = Vector3.Distance(Camera.main.transform.position, transform.position);
        Vector3 startPoint = ray.GetPoint(dist);
        RaycastHit hit;
        if (Physics.Raycast(startPoint, ray.direction, out hit, maxRange, ~ignoredLayers))
        {
            CreateLaser(MidPoint(startPoint, hit.point), ray.direction, Vector3.Distance(startPoint, hit.point));

            EnergyBlastedEffect effect = hit.collider.gameObject.GetComponent<EnergyBlastedEffect>();
            if (effect)
            {
                effect.TriggerEffect();
            }
        }
        else // If no objects in raycast, still shoot laser
        {
            CreateLaser(MidPoint(startPoint, ray.GetPoint(maxRange)), ray.direction, maxRange - dist);
        }
    }

    private void Recharge()
    {
        
            if (currRechargeTimer < rechargeTimer)
            {
                currRechargeTimer += Time.deltaTime;
            }
            else
            {
                currRechargeTimer = 0f;
            }
              OnChargeTimer.Invoke();
        
    }

    private Vector3 MidPoint(Vector3 start, Vector3 end)
    {
        return new Vector3(
            (start.x + end.x) / 2,
            (start.y + end.y) / 2,
            (start.z + end.z) / 2
        );
    }

    private void CreateLaser(Vector3 pos, Vector3 rot, float dist)
    {
        GameObject laser = Instantiate(energyBlast, pos, Quaternion.identity);
        laser.transform.up = rot;

        float targetSize = dist;
        float currentSize = laser.GetComponent<BoxCollider>().bounds.size.y;

        Vector3 scale = laser.transform.localScale;

        scale.y = targetSize * scale.y / currentSize;

        laser.transform.localScale = scale;

        Destroy(laser, 1f);
    }
}
