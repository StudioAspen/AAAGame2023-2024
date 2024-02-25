using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;
using UnityEngine.UIElements;

public class SlashAction : PlayerAction
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
    private BasicMovementAction basicMovementAction;
    private DashMovement dashMovement;
    private SlideAction slideAction;

    private bool isSlashing = false;

    private void Start()
    {
        // Getting Components
        rb = GetComponent<Rigidbody>();
        movementModification = GetComponent<MovementModification>();
        basicMovementAction = GetComponent<BasicMovementAction>();
        dashMovement = GetComponent<DashMovement>();
        slideAction = GetComponent<SlideAction>();

        swordMovement.OnContact.AddListener(SlashContact);
    }


    public void SlashInput() {
        //Animation Stuff (to be implemented later)
        isSlashing = true;

        // Demon sword variables
        swordMovement.OnEndAction.AddListener(EndOfSlashAnimation);
        swordMovement.AttackPosition(attackDuration);
    }

    public void SlashContact(Collider other) {
        Vector3 slashDirection = transform.forward;

        if (other.TryGetComponent(out Slashable slashable) && isSlashing) {
            slashable.TriggerEffect();
        }

        if (other.TryGetComponent(out PathCreator pathCreator) && isSlashing) {
            if(other.TryGetComponent(out WallDirection wallDir)) {
                Vector3 wallForward = wallDir.GetForwardVector();

                //Dot gives a value comparing the two directions, 1 = same direction, -1 = opposite direction
                float directionCheck = Vector3.Dot(slashDirection.normalized, wallForward.normalized);

                if (directionCheck < 0 && isSlashing) {
                    StartSlideAction(pathCreator, other);
                }
            }
            else {
                StartSlideAction(pathCreator, other);
            }
        }
    }

    private void EndOfSlashAnimation() {
        isSlashing = false;
        swordMovement.OnEndAction.RemoveListener(EndOfSlashAnimation);
    }
    private void StartSlideAction(PathCreator pc, Collider other) {
        isSlashing = false;

        EndAction();
        GetComponent<PlayerActionManager>().ChangeAction(slideAction);
        slideAction.StartSlide(pc, other, slideSpeed);
    }

    public bool CanPerformSlash() {
        return !isSlashing;
    }
    public override void EndAction() {
        isSlashing = false;
        OnEndAction.Invoke();
    }
}
