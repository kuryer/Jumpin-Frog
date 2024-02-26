using UnityEngine;

public class GravityController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GravityStateVariable ActualState;
    [SerializeField] GravityControllerRuntimeValue RuntimeValue;

    #region Runtime Value Assign

    private void OnEnable()
    {
        if (RuntimeValue != null)
            RuntimeValue.SetItem(this);
    }

    private void OnDisable()
    {
        if(RuntimeValue != null)
            RuntimeValue.NullItem();
    }

    #endregion

    public void ChangeGravity(GravityState gravityState)
    {
        if (ActualState.Value == gravityState)
            return;
        rb.gravityScale = gravityState.Gravity;
        ActualState.Value = gravityState;
    }
}
