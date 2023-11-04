using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Killable : MonoBehaviour
{
    public float maxHP;
    public float currentHP;

    UnityEvent OnTakeDamage = new UnityEvent();
    UnityEvent OnDie = new UnityEvent();

    public void TakeDamage(float amount)
    {
        currentHP = Mathf.Max(currentHP-amount, 0); // Limitting to 0
        if (OnTakeDamage == null)
        {
            Debug.Log("Killable component doesn't have UnityEvent OnTakeDamage"); 
        } else
        {
            OnTakeDamage.Invoke();
        }
        Debug.Log("killable object took " + amount + " damage");
        if(currentHP <= 0)
        {
            if (OnDie == null)
            {
                Debug.Log("Killable component doesn't have UnityEvent OnDie");
            } else
            {
                OnDie.Invoke();
            }
        }
    }
}
