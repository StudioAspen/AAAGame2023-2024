using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;
using UnityEngine.UIElements;

public class Slash : MonoBehaviour
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

    private PathCreator pathCreator;
    private EndOfPathInstruction end;
    private float dstTravelled = 0f;

    private bool sliding = false;
    private bool isSlashing = false;
    private Vector3 playerOffset;
    private Vector3 swordOffset;

    private void Start()
    {
        // Getting Components
        rb = GetComponent<Rigidbody>();
        movementModification = GetComponent<MovementModification>();
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        dashMovement = GetComponent<DashMovement>();

        end = EndOfPathInstruction.Stop;
        swordMovement.OnContact.AddListener(SlashContact);
    }

    private void Update()
    {
        if (sliding)
        {
            UpdateSliding();
        }
    }

    private void UpdateSliding()
    {
        dstTravelled += Mathf.Lerp(slideSpeed, boostedSlideSpeed, movementModification.boostForAll) * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + playerOffset;
        swordObject.transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + swordOffset;
        swordObject.transform.up = pathCreator.path.GetNormalAtDistance(dstTravelled, end);

        if (dstTravelled > pathCreator.path.length) {
            playerMovement.Jump(Mathf.Lerp(jumpMultiplier, boostedJumpMultiplier, movementModification.boostForAll));
            EndSlide();
        }
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
        playerInput.DisableInput();
        GetComponent<BloodThirst>().GainBlood(bloodGained, true);

        // Slide Initalization
        sliding = true;
        Vector3 contactPoint = other.ClosestPoint(swordObject.transform.position);
        pathCreator = pc;
        rb.useGravity = false;
        dstTravelled = pathCreator.path.GetClosestDistanceAlongPath(contactPoint);
        playerOffset = transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);
        swordOffset = swordObject.transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);
    }

    private void EndSlide() {
        playerMovement.ResetJump();
        dashMovement.ResetDash();
        playerInput.EnableInput();
        dstTravelled = 0f;
        sliding = false;
        rb.useGravity = true;
    }

    public void SlashContact(Collider other)
    {
        if (other.TryGetComponent(out Slashable slashable)) {
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
    public void InterruptSlash()
    {
        EndSlide();
    }

}
