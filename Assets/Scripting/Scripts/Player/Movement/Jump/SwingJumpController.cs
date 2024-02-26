using UnityEngine;

public class SwingJumpController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerVarsSO playerVariables;

    [Header("Jump")]
    [SerializeField] SwingReleaseAction SwingRelease;

    public void SwingJump()
    {
        SwingRelease.SwingRelease();
        //spawn Jump particles
        // earlier there was X here as an input X instead of abs(rb.vel.x)
        Vector2 direction = new Vector2(SwingJumpDirectionX() * Mathf.Abs(rb.velocity.x) * playerVariables.swingJumpBalance,
            SwingJumpDirectionY() * (1f - playerVariables.swingJumpBalance));
        rb.AddForce(direction * playerVariables.swingJumpForce /** movePercent*/, ForceMode2D.Impulse);
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