using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SausageSlashable : Slashable
{
    public int bloodAmount;
    public int stunnedBloodAmount;
    public GameObject bloodPrefab;
    public GameObject bulletFirePoint;

    override public void TriggerEffect()
    {
        /*
         * Slash and Slash Dash both kill the enemy instantly. 
         * Upon death the enemy drops a specified amount of blood and 
         * grants a temporary speed boost to the player for a specified number of seconds. 
         */
        Debug.Log("slashed");
        if(gameObject.GetComponent<SausageEnergyBlast>().isStunned)
        {
            for (int i = 0; i < stunnedBloodAmount; i++) 
            {
                Instantiate(bloodPrefab, bulletFirePoint.transform.position, Quaternion.identity);
                gameObject.GetComponent<EnemyStateManager>().Death();
            }
        }
        else
        {
            for (int i = 0; i < bloodAmount; i++)
            {
                Instantiate(bloodPrefab, bulletFirePoint.transform.position, Quaternion.identity);
                gameObject.GetComponent<EnemyStateManager>().Death();
            }
        }
        
    }
}
