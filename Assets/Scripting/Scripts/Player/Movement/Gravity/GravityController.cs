using UnityEngine;

public class GravityController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GravityState ActualState;

    public void ChangeGravity(GravityState gravityState)
    {
        if (ActualState == gravityState)
            return;
        rb.gravityScale = gravityState.Gravity;
    }
}
