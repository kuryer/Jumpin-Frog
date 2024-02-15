using UnityEngine;

public class FallDetection : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameEvent OnFallEvent;
    public void FallCheck()
    {
        if(rb.velocity.y < 0)
            OnFallEvent.Raise();
    }
}