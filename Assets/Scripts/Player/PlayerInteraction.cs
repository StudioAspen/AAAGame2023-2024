using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // the radius of sphere
    public float interactDistance;
    //public string interactKey;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        // gets all objects around the player and if they have a interact script
        // run that overriden interact script
        Collider[] interactObjectCollider = Physics.OverlapSphere(transform.position, interactDistance);

        foreach (Collider objectInteract in interactObjectCollider)
        {
            Interactable interactible;
            if(objectInteract.TryGetComponent(out interactible))
                interactible.Interact();
        }
    }

    // draw gizmos to visualize the interactDistance in the scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position, interactDistance);
    }
}
