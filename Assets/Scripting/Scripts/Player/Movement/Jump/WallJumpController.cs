using UnityEngine;

public class WallJumpController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerVarsSO playerVariables;

    [Header("Jump Variables")]
    [SerializeField] Buffer WallJumpBuffer;
    [SerializeField] BuffersController BuffersController;
    [SerializeField] DirectionBool WallDetectionBool;
    [SerializeField] MovementStateVariable ActualState;

    [Header("Gravity")]
    [SerializeField] GravityController GravityController;
    [SerializeField] GravityState NormalGravity;

    [Header("State Management")]
    [SerializeField] MovementStateMachine StateMachine;
    [SerializeField] MovementState InAirMovementState;

    [Header("Anti-Double Jump")]
    [SerializeField] WallDetection WallDetection;
    
    void Update()
    {
        WallJumpCheck();
    }

    void WallJumpCheck()
    {
        if (WallJumpBuffer.ActivityInfo.Value() && WallDetectionBool.LeftOrRight())
            WallJump();
    }

    void WallJump()
    {
        if (ActualState.Value is OnWallMovement)
            StateMachine.ChangeState(InAirMovementState);
        
        GravityController.ChangeGravity(NormalGravity);

        if(WallDetectionBool.Left)
            LeftJump();
        else if(WallDetectionBool.Right)
            RightJump();

        BuffersController.ResetWallJumpBuffer();
    }

    void LeftJump()
    {
        rb.AddForce(((Vector2.up * playerVariables.wallJumpDirectionBalance) + (Vector2.right * (1f - playerVariables.wallJumpDirectionBalance))) * playerVariables.jumpPower * playerVariables.wallJumpPowerModifier, ForceMode2D.Impulse);
        WallDetection.LeftWallJumped();
    }

    void RightJump()
    {
        rb.AddForce(((Vector2.up * playerVariables.wallJumpDirectionBalance) + (Vector2.left * (1f - playerVariables.wallJumpDirectionBalance))) * playerVariables.jumpPower * playerVariables.wallJumpPowerModifier, ForceMode2D.Impulse);
        WallDetection.RightWallJumped();
    }
}