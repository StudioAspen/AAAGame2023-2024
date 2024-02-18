using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal.Internal;

public class EnergyBlast : MonoBehaviour
{
    [System.NonSerialized]
    public UnityEvent OnChargesChanged = new UnityEvent();

    public GameObject energyBlast;
    public RectTransform reticle;
    public LayerMask ignoredLayers;

    public float maxRange;
    public int maxNumOfCharges;
    public float rechargeTimer;
    public float timeBetweenShots;

    [System.NonSerialized]
    public int currNumOfCharges;

    private float currRechargeTimer = 0f;
    private float currTimeBetweenShots;

    private void Start()
    {
        currNumOfCharges = maxNumOfCharges;
        currTimeBetweenShots = timeBetweenShots;
        OnChargesChanged.Invoke();
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
        if (currTimeBetweenShots >= timeBetweenShots && currNumOfCharges > 0)
        {
            ShootInternal();
            currTimeBetweenShots = 0f;
            currRechargeTimer = 0f;
            currNumOfCharges--;
            OnChargesChanged.Invoke();
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
        if (currNumOfCharges < maxNumOfCharges)
        {
            if (currRechargeTimer < rechargeTimer)
            {
                currRechargeTimer += Time.deltaTime;
            }
            else
            {
                currNumOfCharges++;
                currRechargeTimer = 0f;
                OnChargesChanged.Invoke();
            }
        }
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
