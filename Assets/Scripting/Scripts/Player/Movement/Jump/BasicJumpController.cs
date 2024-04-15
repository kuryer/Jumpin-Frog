using UnityEngine;

public class BasicJumpController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerMovementVariables playerVariables;

    [Header("Jump")]
    [SerializeField] Buffer JumpBuffer;
    [SerializeField] BoolVariable isGrounded;
    [SerializeField] BuffersController BuffersController;
    [SerializeField] JumpCutController JumpCutController;

    [Header("State Management")]
    [SerializeField] MovementStateMachine StateMachine;
    [SerializeField] MovementState InAirMovementState;

    [Header("Anti-Double Jump")]
    [SerializeField] GroundDetection GroundDetection;

    [Header("Animation Management")]
    [SerializeField] AnimationController AnimationController;
    [SerializeField] AnimationState JumpAnimationState;

    [Header("Gravity Management")]
    [SerializeField] GravityControllerRuntimeValue gravityController;
    [SerializeField] GravityState InAirGravityState;
    void Update()
    {
        BasicJumpCheck();
    }

    void BasicJumpCheck()
    {
        if (JumpBuffer.ActivityInfo.Value() && isGrounded.Value) 
            BasicJump(playerVariables.JumpForce);
    }

    public void BasicJump(float jumpForce)
    {
        BodyTypeCheck();
        gravityController.Item.ChangeGravity(InAirGravityState);
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        BuffersController.ResetJumpBuffer();
        StateMachine.ChangeState(InAirMovementState);
        GroundDetection.PlayerJumpedCall(); // Anti-double Jump
        JumpCutController.enabled = true;
        AnimationController.ChangeAnimation(JumpAnimationState);
    }

    void BodyTypeCheck()
    {
        if (rb.bodyType == RigidbodyType2D.Kinematic)
            rb.bodyType = RigidbodyType2D.Dynamic;
    }
}