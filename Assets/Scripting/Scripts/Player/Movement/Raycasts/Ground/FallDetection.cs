using UnityEngine;

public class FallDetection : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameEvent OnFallEvent;
    [SerializeField] BoolVariable isFalling;
    [SerializeField] MovementStateVariable ActualState;

    [Header("Animation Management")]
    [SerializeField] AnimationController AnimationController;

    void Update()
    {
        FallCheck();
    }

    public void FallCheck()
    {
        if (rb.velocity.y < 0)
            if (isFalling.Value) 
                return;
            else
                FallCall();
        else
            isFalling.Value = false;
    }

    void FallCall()
    {
        isFalling.Value = true;
        OnFallEvent.Raise();
    }

    private void OnDisable()
    {
        isFalling.Value = false;
    }
}