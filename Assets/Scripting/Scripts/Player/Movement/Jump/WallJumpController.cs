using UnityEngine;

public class WallJumpController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerVarsSO playerVariables;

    [Header("Jump Variables")]
    [SerializeField] Buffer WallJumpBuffer;
    [SerializeField] BuffersController BuffersController;
    [SerializeField] CombinedBool WallDetectionBool;
    [SerializeField] MovementStateVariable ActualState;

    [Header("Anti-Double Jump")]
    [SerializeField] WallDetection WallDetection;
    
    void Update()
    {
        WallJumpCheck();
    }

    void WallJumpCheck()
    {
        if (WallJumpBuffer.ActivityInfo.Value() && WallDetectionBool.Or())
            WallJump();
    }

    void WallJump()
    {
        if (ActualState.Value is OnWallMovement)
            ;//call change state machine to in air;

        if(WallDetectionBool.FirstValue)
            LeftJump();
        else if(WallDetectionBool.SecondValue)
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