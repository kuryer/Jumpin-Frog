using UnityEngine;
using UnityEngine.InputSystem;

public class SwingJumpController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerMovementVariables playerVariables;

    [Header("Jump")]
    [SerializeField] SwingReleaseAction SwingRelease;

    [Header("Jump Detection")]
    [SerializeField] SwingVariable ActualSwing;

    private void Awake()
    {
        enabled = false;
    }


    public void SwingJumpCall(InputAction.CallbackContext context)
    {
        if (!context.started || !enabled)
            return;
        bool swingCheck = SwingJumpCheck();
        SwingRelease.SwingRelease();
        //spawn Jump particles
        if(swingCheck)
            SwingJump();
    }

    bool SwingJumpCheck()
    {
        if (playerVariables.hasJumpArea)
            return AreaJumpCheck();
        else
            return VelocityJumpCheck();
    }

    bool AreaJumpCheck()
    {
        float angle = Vector2.Angle(transform.parent.position - ActualSwing.Value.transform.position, Vector2.right);
        if (angle > playerVariables.maxLeftAngle || angle < playerVariables.maxRightAngle)
            return true;
        return false;
    }

    bool VelocityJumpCheck()
    {
        if(rb.velocity.sqrMagnitude > playerVariables.thresholdVel)
            return true;
        return false;
    }

    void SwingJump()
    {
        Debug.Log("Swing Jumped");
        Vector2 direction = new Vector2(SwingJumpDirectionX() * Mathf.Sign(rb.velocity.x) * playerVariables.SwingJumpBalance,
                SwingJumpDirectionY() * (1f - playerVariables.SwingJumpBalance));

        rb.AddForce(direction * playerVariables.SwingJumpForce, ForceMode2D.Impulse);
    }
    float SwingJumpDirectionX()
    {
        if (Mathf.Abs(rb.velocity.x) > 2f)
            return 2f;
        else
            return Mathf.Abs(rb.velocity.x);
    }
    float SwingJumpDirectionY()
    {
        if (rb.velocity.y < 0f)
            return 0f;
        if (rb.velocity.y > 2f)
            return 2f;
        return rb.velocity.y;
    }
}