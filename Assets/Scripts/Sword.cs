using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public SlashAndSlide player;

    private void OnTriggerEnter(Collider collision)
    {
        player.SlashContact(collision.gameObject, collision.ClosestPoint(transform.position));
    }
}
