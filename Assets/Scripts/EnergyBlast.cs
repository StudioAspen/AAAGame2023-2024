using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class EnergyBlast : MonoBehaviour
{
    public RectTransform reticle;

    public float maxRange;
    public int maxNumOfCharges;
    public float rechargeTimer;
    public float timeBetweenShots;

    private int currNumOfCharges;
    private float currRechargeTimer = 0f;
    private float currTimeBetweenShots;

    private void Start()
    {
        currNumOfCharges = maxNumOfCharges;
        currTimeBetweenShots = timeBetweenShots;
    }

    private void Update()
    {
        Recharge();

        if (currTimeBetweenShots < timeBetweenShots)
        {
            currTimeBetweenShots += Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && currTimeBetweenShots >= timeBetweenShots && currNumOfCharges > 0)
        {
            Shoot();
            currTimeBetweenShots = 0f;
            currRechargeTimer = 0f;
            currNumOfCharges--;
        }
    }

    private void Shoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(reticle.position);
        float dist = Vector3.Distance(Camera.main.transform.position, transform.position);
        Vector3 startPoint = ray.GetPoint(dist);
        Vector3 dir = Camera.main.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(startPoint, dir, out hit, maxRange))
        {
            Debug.DrawLine(startPoint, hit.point, Color.white, 5f);
            Debug.Log("Found an object - distance: " + hit.distance);

            EnergyBlastedEffect effect = hit.collider.gameObject.GetComponent<EnergyBlastedEffect>();
            if (effect)
            {
                effect.TriggerEffect();
            }
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
            }
        }
    }
}
