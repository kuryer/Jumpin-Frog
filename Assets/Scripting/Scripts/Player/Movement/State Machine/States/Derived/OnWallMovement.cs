using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player/Movement States/On Wall Movement")]
public class OnWallMovement : MovementState
{
    [Header("Player")]
    [SerializeField] Rigidbody2DRuntimeValue rb;
    [SerializeField] PlayerMovementVariables playerVariables;
    [SerializeField] DirectionBool DetectedWalls;
    [SerializeField] FloatVariable X;
    [SerializeField] GameEvent ExitOnWallEvent;

    [Header("State Management")]
    [SerializeField] MovementStateMachineRuntimeValue StateMachine;
    [SerializeField] MovementState InAirMovementState;

    [Header("Gravity")]
    [SerializeField] GravityControllerRuntimeValue gravityController;
    [SerializeField] GravityState WallGlideGravity;
    [SerializeField] GravityState WallGrabGravity;
    bool isFalling;

    [Header("Animation Management")]
    [SerializeField] AnimationControllerRuntimeValue AnimationControllerValue;
    [SerializeField] AnimationState OnWallAnimationState;
    [SerializeField] AnimationState JumpAnimationState;

    [Header("Disabler")]
    [SerializeField] DisablerEvent OnWallMovementDisablerEvent;


    public override void OnEnter()
    {
        OnWallMovementDisablerEvent.SetScripts(true);
        AnimationControllerValue.Item.ChangeAnimation(OnWallAnimationState);
    }
    public override void OnUpdate()
    {
        if (isStillGrabbing())
            return;
        StopGrabbing();
    }
    public override void OnFixedUpdate()
    {
        WallGrab();
        WallGrabGravityCheck();
    }
    public override void OnExit()
    {
        OnWallMovementDisablerEvent.SetScripts(false);
        ExitOnWallEvent.Raise(); //Changes gravity to normal
        isFalling = false;
        AnimationControllerValue.Item.ChangeAnimation(JumpAnimationState);
    }

    #region Wall Grab Check
    bool isStillGrabbing()
    {
        if (DetectedWalls.Left && X.Value == -1)
            return true;
        if (DetectedWalls.Right && X.Value == 1)
            return true;
        return false;
    }

    void StopGrabbing()
    {
        StateMachine.Item.ChangeState(InAirMovementState);
    }
    #endregion

    #region Wall Grab

    void WallGrab()
    {
        if (rb.Item.velocity.y < -playerVariables.wallGrabClamp)
            rb.Item.velocity = new Vector2(XGrabValue(), -playerVariables.wallGrabClamp);
    }

    float XGrabValue()
    {
        return X.Value * playerVariables.wallGrabXForce;
    }
    void WallGrabGravityCheck()
    {
        if (isFalling)
            return;

        if (rb.Item.velocity.y < playerVariables.wallGravityVelocityChange)
            ChangeWallGravityToGrab();
    }

    public void ChangeWallGravityToGrab()
    {
        isFalling = true;
        gravityController.Item.ChangeGravity(WallGrabGravity);
    }

    #endregion
}
