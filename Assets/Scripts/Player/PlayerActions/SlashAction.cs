using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;
using UnityEngine.UIElements;

public class SlashAction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject swordObject;
    [SerializeField] SwordMovement swordMovement;

    [Header("Movement Variables")]
    public float slideSpeed;
    public float jumpMultiplier;

    [Header("Boosted Movement")]
    public float boostedSlideSpeed;
    public float boostedJumpMultiplier;

    [Header("Other Variables")]
    public float attackDuration;
    public float bloodGained;

    [Header("Events")]
    public UnityEvent OnSlashEnd = new UnityEvent();

    // Components
    private MovementModification movementModification;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private DashMovement dashMovement;


    private bool sliding = false;
    private bool isSlashing = false;

    private void Start()
    {
        // Getting Components
        rb = GetComponent<Rigidbody>();
        movementModification = GetComponent<MovementModification>();
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        dashMovement = GetComponent<DashMovement>();

        swordMovement.OnContact.AddListener(SlashContact);
    }


    public void StartSlash() {
        if (!isSlashing) {
            //Animation Stuff (to be implemented later)
            isSlashing = true;

            // Demon sword variables
            swordMovement.OnEndAction.AddListener(EndOfSlashAnimation);
            swordMovement.AttackPosition(attackDuration);
        }
    }

    public void StartSlide(PathCreator pc, Collider other) {
        GetComponent<BloodThirst>().GainBlood(bloodGained, true);

    }

    public void SlashContact(Collider other)
    {
        if (other.TryGetComponent(out Slashable slashable) && isSlashing) {
            slashable.TriggerEffect();
        }

        if(other.TryGetComponent(out PathCreator pc)) {
            if (!sliding && isSlashing) {
                isSlashing = false;
                StartSlide(pc, other);
            }
        }
    }
    private void EndOfSlashAnimation() {
        isSlashing = false;
        swordMovement.OnEndAction.RemoveListener(EndOfSlashAnimation);
    }

}
