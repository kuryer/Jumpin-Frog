using UnityEngine;

public class JumpPadJump : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] MovementStateVariable ActualState;

    [Header("State Management")]
    [SerializeField] MovementStateMachine StateMachine;
    [SerializeField] MovementState InAirMovementState;

    [Header("Gravity Management")]
    [SerializeField] GravityController GravityController;
    [SerializeField] GravityState NormalGravity;

    [Header("Anti-Double Jump")]
    [SerializeField] GroundDetection GroundDetection;

    [Header("Animation Management")]
    [SerializeField] AnimationController AnimationController;
    [SerializeField] AnimationState JumpAnimationState;

    public void Jump(float jumpForce)
    {
        BodyTypeCheck();
        GravityController.ChangeGravity(NormalGravity);
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        AnimationController.ChangeAnimation(JumpAnimationState);
        if (ActualState.Value is GroundMovementState)
            TransitionToInAir();
    }

    void TransitionToInAir()
    {
        StateMachine.ChangeState(InAirMovementState);
        GroundDetection.PlayerJumpedCall();
    } 

    void BodyTypeCheck()
    {
        if (rb.bodyType == RigidbodyType2D.Kinematic)
            rb.bodyType = RigidbodyType2D.Dynamic;
    }
}