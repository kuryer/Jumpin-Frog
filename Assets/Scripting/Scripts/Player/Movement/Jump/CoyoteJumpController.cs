using UnityEngine;

public class CoyoteJumpController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerMovementVariables playerVariables;

    [Header("Jump")]
    [SerializeField] Buffer JumpBuffer;
    [SerializeField] Buffer CoyoteTime;
    [SerializeField] BuffersController BuffersController;
    [SerializeField] JumpCutController JumpCutController;

    [Header("Animation Management")]
    [SerializeField] AnimationController AnimationController;
    [SerializeField] AnimationState JumpAnimationState;

    [Header("Gravity Management")]
    [SerializeField] GravityControllerRuntimeValue gravityController;
    [SerializeField] GravityState InAirGravityState;
    [SerializeField] CoyoteHangTimer hangTimer;

    [Header("Camera Playtesting")]
    [SerializeField] CameraStateEvent InAirCameraStateEvent;
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
        hangTimer.enabled = false;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        gravityController.Item.ChangeGravity(InAirGravityState);
        rb.AddForce(Vector2.up * playerVariables.JumpForce, ForceMode2D.Impulse);
        ResetBuffers();
        JumpCutController.enabled = true;
        InAirCameraStateEvent.Raise();
        AnimationController.ChangeAnimation(JumpAnimationState);
    }

    void ResetBuffers()
    {
        BuffersController.ResetJumpBuffer();
        BuffersController.ResetCoyoteTime();
    }
}
