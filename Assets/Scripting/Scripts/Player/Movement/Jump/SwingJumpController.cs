using UnityEngine;
using UnityEngine.InputSystem;

public class SwingJumpController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerMovementVariables playerVariables;

    [Header("Jump")]
    [SerializeField] SwingReleaseAction SwingRelease;

    private void Awake()
    {
        enabled = false;
    }

    public void SwingJump(InputAction.CallbackContext context)
    {
        if (!context.started || !enabled)
            return;
        SwingRelease.SwingRelease();
        Debug.Log("swing jump");
        //spawn Jump particles
        // earlier there was X here as an input X instead of sign(rb.vel.x)
        Vector2 direction = new Vector2(SwingJumpDirectionX() * Mathf.Sign(rb.velocity.x) * playerVariables.SwingJumpBalance,
            SwingJumpDirectionY() * (1f - playerVariables.SwingJumpBalance));
        rb.AddForce(direction * playerVariables.SwingJumpForce /** movePercent*/, ForceMode2D.Impulse);
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