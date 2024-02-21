using UnityEngine;

public class WallDetection : MonoBehaviour
{
    [Header("Ray Values")]
    [SerializeField] Vector3 RayPosition;
    [SerializeField] Vector3 RayOffset;
    [SerializeField] float RayDistance;
    [SerializeField] LayerMask WallLayer;
    
    [Header("Detection Values")]
    [SerializeField] DirectionBool DetectedWalls;
    private delegate void WallDetectionDelegate();
    WallDetectionDelegate leftWallDetection;
    WallDetectionDelegate rightWallDetection;

    #region Updates
    private void Awake()
    {
        leftWallDetection = DefaultLeftWallCheck;
        rightWallDetection = DefaultRightWallCheck;
    }

    void FixedUpdate()
    {
        leftWallDetection();
        rightWallDetection();
    }
    #endregion

    #region Raycast

    bool RightWallCheck()
    {
        return Physics2D.Raycast(transform.position + RayPosition + RayOffset, Vector3.right, RayDistance, WallLayer) ||
            Physics2D.Raycast(transform.position + RayPosition + (2 * RayOffset), Vector3.right, RayDistance, WallLayer) ||
            Physics2D.Raycast(transform.position + RayPosition - (2 * RayOffset), Vector3.right, RayDistance, WallLayer) ||
            Physics2D.Raycast(transform.position + RayPosition - RayOffset, Vector3.right, RayDistance, WallLayer);
    }

    bool LeftWallCheck()
    {
        return Physics2D.Raycast(transform.position + RayPosition + RayOffset, Vector3.left, RayDistance, WallLayer) ||
            Physics2D.Raycast(transform.position + RayPosition + (2 * RayOffset), Vector3.left, RayDistance, WallLayer) ||
            Physics2D.Raycast(transform.position + RayPosition - (2 * RayOffset), Vector3.left, RayDistance, WallLayer) ||
            Physics2D.Raycast(transform.position + RayPosition - RayOffset, Vector3.left, RayDistance, WallLayer);
    }

    #endregion

    #region JumpCheckDefault

    void DefaultLeftWallCheck()
    {
        DetectedWalls.Left = LeftWallCheck();
    }
    void DefaultRightWallCheck()
    {
        DetectedWalls.Right = RightWallCheck();
    }

    #endregion

    #region JumpCheckJumped

    #region Left

    public void LeftWallJumped()
    {
        DetectedWalls.Left = false;
        leftWallDetection = LeftWallJumpedCheck;
    }
    void LeftWallJumpedCheck()
    {
        //tu mo¿na dla beki wsadzic podobny mechanizm co w ground detection, z tym "timerem"
        if (RightWallCheck() == false)
            LeftReturnToDefault();
    }

    void LeftReturnToDefault()
    {
        leftWallDetection = DefaultLeftWallCheck;
    }

    #endregion

    #region Right
    public void RightWallJumped()
    {
        DetectedWalls.Right = false;
        rightWallDetection = RightWallJumpedCheck;
    }
    void RightWallJumpedCheck()
    {
        //tu mo¿na dla beki wsadzic podobny mechanizm co w ground detection, z tym "timerem"
        if (RightWallCheck() == false)
            RightReturnToDefault();
    }

    void RightReturnToDefault()
    {
        rightWallDetection = DefaultRightWallCheck;
    }

    #endregion

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + RayPosition + RayOffset, Vector3.right * RayDistance);
        Gizmos.DrawRay(transform.position + RayPosition + (2 * RayOffset), Vector3.right * RayDistance);
        Gizmos.DrawRay(transform.position + RayPosition - (2 * RayOffset), Vector3.right * RayDistance);
        Gizmos.DrawRay(transform.position + RayPosition - RayOffset, Vector3.right * RayDistance);
        Gizmos.DrawRay(transform.position + RayPosition + RayOffset, Vector3.left * RayDistance);
        Gizmos.DrawRay(transform.position + RayPosition + (2 * RayOffset), Vector3.left * RayDistance);
        Gizmos.DrawRay(transform.position + RayPosition - (2 * RayOffset), Vector3.left * RayDistance);
        Gizmos.DrawRay(transform.position + RayPosition - RayOffset, Vector3.left * RayDistance);
    }
}