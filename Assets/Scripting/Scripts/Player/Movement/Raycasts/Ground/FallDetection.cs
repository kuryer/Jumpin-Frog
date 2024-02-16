using UnityEngine;

public class FallDetection : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameEvent OnFallEvent;
    [SerializeField] bool isFalling;
    [SerializeField] MovementStateVariable ActualState;

    void Update()
    {
        if (!(ActualState.Value is InAirMovementState) || isFalling)
            return;

        FallCheck();
    }

    public void FallCheck()
    {
        if(rb.velocity.y < 0)
        {
            isFalling = true;
            OnFallEvent.Raise();
        }
    }
}