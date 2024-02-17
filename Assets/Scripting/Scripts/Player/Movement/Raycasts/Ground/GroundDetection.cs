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
    [SerializeField] BoolVariable PlayerJumped;
    private delegate void GroundCheckDelegate();
    GroundCheckDelegate groundCheckDelegate;


    private void FixedUpdate()
    {

        if (!(ActualState.Value == GroundState || ActualState.Value == InAirState))
            return;

        if(isGrounded.Value != GroundCheck())
        {
            if(isGrounded.Value == true)
                InAirCall();
            else
                OnGroundCall();
        }
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
        PlayerJumped.Value = true;
        groundCheckDelegate = PlayerJumpCheck;
    }

    void PlayerJumpCheck()
    {
        if (GroundCheck() == false)
        {
            ChangedInAirCall();
        }
    }

    void ChangedInAirCall()
    {
        PlayerJumped.Value = false;
        isGrounded.Value = false;
        //groundCheckDelegate = defaultMethod
        //Call EVENT????
    }
}