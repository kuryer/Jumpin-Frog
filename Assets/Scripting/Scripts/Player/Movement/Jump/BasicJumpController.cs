using UnityEngine;

public class BasicJumpController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerVarsSO playerVariables;

    [Header("Jump")]
    [SerializeField] Buffer JumpBuffer;
    [SerializeField] BoolVariable isGrounded;
    [SerializeField] BuffersController BuffersController;

    [Header("State Management")]
    [SerializeField] MovementStateMachine StateMachine;
    [SerializeField] MovementState InAirMovementState;

    [Header("Anti-Double Jump")]
    [SerializeField] GroundDetection GroundDetection;

    void Update()
    {
        BasicJumpCheck();
    }

    void BasicJumpCheck()
    {
        if (JumpBuffer.ActivityInfo.Value() && isGrounded.Value) 
            BasicJump(playerVariables.jumpPower);
    }

    public void BasicJump(float jumpForce)
    {
        BodyTypeCheck();
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        BuffersController.ResetJumpBuffer();
        StateMachine.ChangeState(InAirMovementState);
        GroundDetection.PlayerJumpedCall(); // Anti-double Jump
    }

    void BodyTypeCheck()
    {
        if (rb.bodyType == RigidbodyType2D.Kinematic)
            rb.bodyType = RigidbodyType2D.Dynamic;
    }
}