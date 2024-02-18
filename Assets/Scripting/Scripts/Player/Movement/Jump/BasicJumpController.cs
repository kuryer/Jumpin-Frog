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

    [Header("Anti-Double Jump")]
    [SerializeField] GroundDetection GroundDetection;

    void Update()
    {
        BasicJumpCheck();
    }

    void BasicJumpCheck()
    {
        if (JumpBuffer.ActivityInfo.Value() && isGrounded.Value) 
            BasicJump();
    }

    public void BasicJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * playerVariables.jumpPower, ForceMode2D.Impulse);
        //Reset buffers
        BuffersController.ResetJumpBuffer();
        //Change state to in air
        //anti-double jump mechanic
        GroundDetection.PlayerJumpedCall();
    }
}