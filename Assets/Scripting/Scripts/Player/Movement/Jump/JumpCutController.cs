using UnityEngine;
using UnityEngine.InputSystem;

public class JumpCutController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoolVariable isGrounded;
    [SerializeField] PlayerMovementVariables playerVariables;

    private void Awake()
    {
        enabled = false;
    }

    public void JumpCut(InputAction.CallbackContext context)
    {
        if (!context.canceled || !enabled)
            return;
        if(rb.velocity.y > 0.1f)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * playerVariables.JumpCutMultiplier, ForceMode2D.Impulse);
            enabled = false;
        }
    }
}