using UnityEngine;

public class CoyoteJumpController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerVarsSO playerVariables;

    [Header("Jump")]
    [SerializeField] Buffer JumpBuffer;
    [SerializeField] Buffer CoyoteTime;
    [SerializeField] BuffersController BuffersController;
    [SerializeField] JumpCutController JumpCutController;
    void Update()
    {
        CoyoteJumpCheck();
    }

    void CoyoteJumpCheck()
    {
        if (JumpBuffer.ActivityInfo.Value() && CoyoteTime.ActivityInfo.Value())
            CoyoteJump();
    }

    void CoyoteJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * playerVariables.jumpPower, ForceMode2D.Impulse);
        ResetBuffers();
    }

    void ResetBuffers()
    {
        BuffersController.ResetJumpBuffer();
        BuffersController.ResetCoyoteTime();
    }
}
