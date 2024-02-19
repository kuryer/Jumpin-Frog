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
            BasicJump(playerVariables.jumpPower);
    }

    public void BasicJump(float jumpForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        BuffersController.ResetJumpBuffer();
        //Change state to in air
        GroundDetection.PlayerJumpedCall(); // Anti-double Jump
    }
}