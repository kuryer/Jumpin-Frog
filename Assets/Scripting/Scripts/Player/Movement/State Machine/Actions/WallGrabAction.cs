using UnityEngine;

public class WallGrabAction : MonoBehaviour
{
    [Header("WallGrab Check")]
    [SerializeField] DirectionBool DetectedWalls;
    [SerializeField] FloatVariable X;

    [Header("Wall Grab")]
    [SerializeField] Rigidbody2D rb;

    [Header("State Management")]
    [SerializeField] MovementStateMachine StateMachine;
    [SerializeField] MovementState OnWallMovementState;

    [Header("Gravity")]
    [SerializeField] GravityController GravityController;
    [SerializeField] GravityState WallGrabGravity;

    [Header("Stick To Wall")]
    [SerializeField] float initialStickForce;
    // Update is called once per frame
    void Update()
    {
        if (IsGrabbingProperSide())
            WallGrab();
    }

    bool IsGrabbingProperSide()
    {
        if(X.Value == 1 && DetectedWalls.Right)
            return true;
        if(X.Value == -1 && DetectedWalls.Left)
            return true;
        return false;
    }

    void WallGrab()
    {
        StickToWall();
        GravityController.ChangeGravity(WallGrabGravity);
        StateMachine.ChangeState(OnWallMovementState);
    }

    void StickToWall()
    {
        rb.velocity = new Vector2(X.Value * initialStickForce, 0f);
    }
}
