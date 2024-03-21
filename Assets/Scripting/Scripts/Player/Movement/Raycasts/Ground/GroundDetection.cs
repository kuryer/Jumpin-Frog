using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    [Header("Ray Values")]
    [SerializeField] Vector3 RayPosition;
    [SerializeField] Vector3 RayOffset;
    [SerializeField] float RayDistance;
    [SerializeField] LayerMask GroundLayer;

    [Header("Event Values")]
    [SerializeField] BoolVariable isGrounded;
    [SerializeField] GameEvent OnGroundEvent;
    [SerializeField] GameEvent InAirEvent;

    [Header("State Management")]
    [SerializeField] MovementStateMachine StateMachine;
    [SerializeField] MovementState GroundMovementState;
    [SerializeField] MovementState InAirMovementState;
    [SerializeField] MovementStateVariable ActualState;

    [Header("Anti-Double Jump")]
    [SerializeField] BasicJumpController JumpController;
    [SerializeField] Rigidbody2D rb;
    private delegate void GroundCheckDelegate();
    GroundCheckDelegate groundCheckDelegate;

    [Header("Camera Playtesting")]
    [SerializeField] CameraStateEvent GroundCameraStateEvent;
    [SerializeField] CameraStateEvent InAirCameraStateEvent;

    private void Awake()
    {
        groundCheckDelegate = DefaultCheck;
    }

    private void FixedUpdate()
    {
        groundCheckDelegate();
    }

    bool GroundCheck()
    {
        return Physics2D.Raycast((transform.position + RayPosition), Vector3.down, RayDistance, GroundLayer) ||
        Physics2D.Raycast((transform.position + RayPosition) - (2 * RayOffset), Vector3.down, RayDistance, GroundLayer) ||
        Physics2D.Raycast((transform.position + RayPosition) - RayOffset, Vector3.down, RayDistance, GroundLayer) ||
        Physics2D.Raycast((transform.position + RayPosition) + (2 * RayOffset), Vector3.down, RayDistance, GroundLayer) ||
        Physics2D.Raycast((transform.position + RayPosition) + RayOffset, Vector3.down, RayDistance, GroundLayer);
    }

    void OnGroundCall()
    {
        isGrounded.Value = true;
        StateMachine.ChangeState(GroundMovementState);
        OnGroundEvent.Raise();
        GroundCameraStateEvent.Raise();
    }

    void InAirCall()
    {
        BodyTypeCheck();
        isGrounded.Value = false;
        StateMachine.ChangeState(InAirMovementState);
        InAirEvent.Raise();
    }

    void BodyTypeCheck()
    {
        if (rb.bodyType == RigidbodyType2D.Kinematic)
            rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position + RayPosition, Vector3.down * RayDistance);
        Gizmos.DrawRay(transform.position + RayPosition - (2 * RayOffset), Vector3.down * RayDistance);
        Gizmos.DrawRay(transform.position + RayPosition + (2 * RayOffset), Vector3.down * RayDistance);
        Gizmos.DrawRay(transform.position + RayPosition - RayOffset, Vector3.down * RayDistance);
        Gizmos.DrawRay(transform.position + RayPosition + RayOffset, Vector3.down * RayDistance);
    }

    public void PlayerJumpedCall()
    {
        SetBasicJumpController(false);
        groundCheckDelegate = PlayerJumpCheck;
        InAirCameraStateEvent.Raise();
        //InAirEvent.Raise();
    }

    void DefaultCheck()
    {
        if (!(ActualState.Value is GroundMovementState || ActualState.Value is InAirMovementState))
            return;

        if (isGrounded.Value != GroundCheck())
        {
            if (isGrounded.Value == true)
                InAirCall();
            else
                OnGroundCall();
        }
    }

    void PlayerJumpCheck()
    {
        if (GroundCheck() == false)
            ChangedInAirCall();
        else if(rb.velocity.y <= 0)
            BackToDefaultGrounded();
    }
    void BackToDefaultGrounded()
    {
        OnGroundCall();
        SetBasicJumpController(true);
        groundCheckDelegate = DefaultCheck;
    }
    void ChangedInAirCall()
    {
        isGrounded.Value = false;
        groundCheckDelegate = DefaultCheck;
        SetBasicJumpController(true);
    }

    void SetBasicJumpController(bool enabled)
    {
        JumpController.enabled = enabled;
    }
}