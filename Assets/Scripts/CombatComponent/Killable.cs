using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Killable : MonoBehaviour
{
    public float maxHP;
    public float currentHP;

    UnityEvent OnTakeDamage;
    UnityEvent OnDie;

    public void TakeDamage(float amount)
    {
        currentHP = Mathf.Max(currentHP-amount, 0); // Limitting to 0
        OnTakeDamage.Invoke();
        if(currentHP <= 0)
        {
            OnDie.Invoke();
        }
    }
}
