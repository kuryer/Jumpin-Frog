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



    [Header("Disabler")]
    [SerializeField] DisablerEvent OnWallMovementDisablerEvent;


    public override void OnEnter()
    {
        OnWallMovementDisablerEvent.SetScripts(true);
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
    }
    public override void OnExit()
    {
        OnWallMovementDisablerEvent.SetScripts(false);
        ExitOnWallEvent.Raise(); //Changes gravity to normal
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
            rb.Item.velocity = new Vector2(0f, -playerVariables.wallGrabClamp);
    }

    #endregion
}
