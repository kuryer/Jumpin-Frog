using UnityEngine;

public class FallDetection : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameEvent OnFallEvent;
    [SerializeField] bool isFalling;
    public void FallCheck()
    {
        if(rb.velocity.y < 0)
            OnFallEvent.Raise();
    }


}