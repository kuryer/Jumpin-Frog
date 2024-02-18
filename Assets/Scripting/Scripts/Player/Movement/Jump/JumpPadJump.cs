using UnityEngine;

public class JumpPadJump : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] MovementStateVariable ActualState;
    [Header("Anti-Double Jump")]
    [SerializeField] GroundDetection GroundDetection;
    public void Jump(float jumpForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        if (ActualState.Value is GroundMovementState)
            TransitionToInAir();
    }

    void TransitionToInAir()
    {
        //Change state to inAir
        GroundDetection.PlayerJumpedCall();
    } 
}