using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;//used for displaying damage text

public class killable : MonoBehaviour
{
    public int health = 1; //player health

    public string gameObject = "GameObjectName";//name of the game object
    public GameObject textDisplayDamage;//will be used to display text on damage taken

    public GameObject textDisplayDeath;//will be used to display text on death

    public UnityEvent OnDie;
     
    void Start()
    {
    }
    

    void Update()
    {
        die();
    }


    void takeDamage()
    {
        health--;
        textDisplayDamage.GetComponent<Text>().text = gameObject + "HP: " + health.ToString();
    }

    void die()
    {
        if (health == 0)//on death, trigger whatever killable script that is needed
        {
            textDisplayDeath.GetComponent<Text>().text = gameObject + "died";//if gameObject dies, print
            OnDie.Invoke();//Death effect triggered
        }
    }

    void OnTrigger2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Whatever Can Damage The Player"))
        {
            takeDamage();
        }
    }
}
