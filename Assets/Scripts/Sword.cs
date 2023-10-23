using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public SlashAndSlide player;

    private void OnCollisionEnter(Collision collision)
    {
        player.SlashContact(collision.gameObject);
    }
}
