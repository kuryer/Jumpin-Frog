using System.Collections;
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

    [SerializeField] MovementState GroundState;
    [SerializeField] MovementState InAirState;
    [SerializeField] MovementStateVariable ActualState;

    [Header("Anti-Double Jump")]
    [SerializeField] BasicJumpController JumpController;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float antiBugTime;
    private delegate void GroundCheckDelegate();
    GroundCheckDelegate groundCheckDelegate;
    //Moge jeszcze zrobic tak ze bede wylaczal skrypt basic jumpa
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
        //call State Machine
        OnGroundEvent.Raise();
    }

    void InAirCall()
    {
        isGrounded.Value = false;
        //Call State Machine changeState
        InAirEvent.Raise();
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
    }

    void DefaultCheck()
    {
        if (!(ActualState.Value == GroundState || ActualState.Value == InAirState))
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
        //Call EVENT????
    }

    void SetBasicJumpController(bool enabled)
    {
        JumpController.enabled = enabled;
    }
}