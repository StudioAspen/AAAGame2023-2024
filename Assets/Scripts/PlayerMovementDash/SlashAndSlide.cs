using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;
using UnityEngine.UIElements;

public class SlashAndSlide : MonoBehaviour
{
    [Header("Movement Variables")]
    public float slideSpeed;
    public float jumpMultiplier;
    public float boostedSlideSpeed;

    [Header("Other Variables")]
    public float bloodGained;
    [SerializeField] GameObject swordObject;

    [Header("Events")]
    public UnityEvent OnSlashStart = new UnityEvent();
    public UnityEvent OnSlashEnd = new UnityEvent();

    public UnityEvent OnSlideStart = new UnityEvent();
    public UnityEvent OnSlideEnd = new UnityEvent();

    // Components
    private MovementModification movementModification;
    private Rigidbody rb;
    private DemonSword sword;
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
        end = EndOfPathInstruction.Stop;
        rb = GetComponent<Rigidbody>();
        movementModification = GetComponent<MovementModification>();
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        dashMovement = GetComponent<DashMovement>();

        sword = swordObject.GetComponent<DemonSword>();
        sword.OnContact.AddListener(SlashContact);
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

        if (dstTravelled > pathCreator.path.length)
        {
            EndSlide();
        }
    }
    public void StartSlash() {
        if (!isSlashing) {
            //Animation Stuff (to be implemented later)
            isSlashing = true;

            // Demon sword variables
            sword.OnEndAction.AddListener(EndOfSlashAnimation);
            sword.AttackPosition();
        }
    }

    private void StartSlide()
    {
        OnSlideStart.Invoke();
        sliding = true;
    }

    private void EndSlide() {
        playerMovement.Jump(jumpMultiplier);
        playerMovement.ResetJump();
        dashMovement.ResetDash();
        playerInput.EnableInput();
        OnSlideEnd.Invoke();
        dstTravelled = 0f;
        sliding = false;
        rb.useGravity = true;
    }

    public void SlashContact(Collider other)
    {
        if (other.TryGetComponent<Slashable>(out Slashable slashable) && 
            other.TryGetComponent<PathCreator>(out PathCreator pc))
        {
            if (!sliding && isSlashing) {
                Vector3 contactPoint = other.ClosestPoint(swordObject.transform.position);
                isSlashing = false;
                playerInput.DisableInput(); 
                sword.GetComponent<BloodThirst>().GainBlood(bloodGained, true);

                StartSlide();
                pathCreator = pc;
                rb.useGravity = false;
                dstTravelled = pathCreator.path.GetClosestDistanceAlongPath(contactPoint);
                playerOffset = transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);
                swordOffset = swordObject.transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);

                // Pause slash animation
            }
        }
    }
    private void EndOfSlashAnimation() {
        isSlashing = false;
        sword.OnEndAction.RemoveListener(EndOfSlashAnimation);
    }
    public void InterruptSlide()
    {
        EndSlide();
    }

}
