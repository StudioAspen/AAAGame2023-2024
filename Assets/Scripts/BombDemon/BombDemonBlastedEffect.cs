using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDemonBlastedEffect : EnergyBlastedEffect
{
    public BombDemon bombDemon;
    public float explosionRadius;
    public float explosionDamage;
    public float bloodLoss;
    public override void TriggerEffect()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in collider)
        {
           //if( hit.TryGetComponent<Killable>(out Killable killable))
          //  {
               // killable.TakeDamage(explosionDamage); 
               // Debug.Log("Took Damage");
           // }
          if( hit.TryGetComponent<BloodThirst>(out BloodThirst bloodThirst))
            {
                bloodThirst.LoseBlood(bloodLoss);
            }
          

        }
      
    }

  
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }

}
