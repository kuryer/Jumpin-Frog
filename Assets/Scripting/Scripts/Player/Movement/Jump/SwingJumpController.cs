using UnityEngine;
using UnityEngine.InputSystem;

public class SwingJumpController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerMovementVariables playerVariables;

    [Header("Jump")]
    [SerializeField] SwingReleaseAction SwingRelease;
    [SerializeField] FloatVariable swingDirection;

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
        bool cached = AreaJumpCheck();
        SwingRelease.SwingRelease();
        //spawn Jump particles
        if(cached)
            SwingJump();
    }

    bool AreaJumpCheck()
    {
        //float angle = Vector2.Angle(transform.parent.position - ActualSwing.Value.transform.position, Vector2.right);
        Vector2 dir = transform.parent.position - ActualSwing.Value.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x);
        if (swingDirection.Value > 0)
            return playerVariables.IsInRightSwingJumpArea(angle);
        if(swingDirection.Value < 0)
            return playerVariables.IsInLeftSwingJumpArea(angle);
        return false;
    }

    void SwingJump()
    {
        Vector2 direction = new Vector2(2 * swingDirection.Value * playerVariables.SwingJumpBalance,
                2 * (1f - playerVariables.SwingJumpBalance));

        rb.AddForce(direction * playerVariables.SwingJumpForce, ForceMode2D.Impulse);
    }
}