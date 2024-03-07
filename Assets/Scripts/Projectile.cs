using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // enemy projectile setters
    public float speed;
    public float damage;
    public float destructionTime;

    private BoxCollider collision;
    private float destructionTimeTimer;

    // Start is called before the first frame update
    void Start()
    {
        collision = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
