using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSlashWalls : EnergyBlastedEffect
{
    [SerializeField] private float rotationSmoothness = 1.0f;
    [Range(0, 1)]
    [SerializeField] private float canRotate; // The percentage of a complete rotation before you can do it again

    private Quaternion targetRotation;

    ///-/////////////////////////////////////////////////////////////////
    ///
    void Start()
    {
        targetRotation = transform.rotation;
    }

    ///-/////////////////////////////////////////////////////////////////
    ///
    void Update()
    {
        UpdateRotation();
    }

    ///-/////////////////////////////////////////////////////////////////
    ///
    public override void TriggerEffect()
    {
        if ((transform.eulerAngles - targetRotation.eulerAngles).magnitude < canRotate)
        {
            targetRotation = Quaternion.AngleAxis(180f, transform.up)*targetRotation;
        }
    }

    ///-/////////////////////////////////////////////////////////////////
    ///
    private void UpdateRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothness);
    }
}
