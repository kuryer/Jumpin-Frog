using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    [Header("Ray Values")]
    [SerializeField] Vector3 RayPosition;
    [SerializeField] Vector3 RayOffset;
    [SerializeField] float RayDistance;
    [SerializeField] LayerMask GroundLayer;

    [Header("Event Values")]
    public BoolVariable isGrounded;
    public GameEvent OnGroundEvent;
    public GameEvent InAirEvent;

    private void FixedUpdate()
    {
        if(isGrounded.Value != IsGrounded())
        {
            if(isGrounded.Value == true)
                InAirCall();
            else
                OnGroundCall();
        }
    }

    bool IsGrounded()
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
}