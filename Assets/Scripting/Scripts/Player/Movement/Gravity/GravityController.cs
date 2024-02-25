using UnityEngine;

public class GravityController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GravityStateVariable ActualState;

    public void ChangeGravity(GravityState gravityState)
    {
        if (ActualState.Value == gravityState)
            return;
        rb.gravityScale = gravityState.Gravity;
        ActualState.Value = gravityState;
    }
}
