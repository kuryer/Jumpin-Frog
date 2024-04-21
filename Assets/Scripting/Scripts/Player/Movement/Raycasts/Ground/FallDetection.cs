using UnityEngine;

public class FallDetection : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameEvent OnFallEvent;
    [SerializeField] BoolVariable isFalling;
    [SerializeField] MovementStateVariable ActualState;
    [SerializeField] GravityStateVariable ActualGravityState;
    [SerializeField] GravityState CoyoteHangGravity;
    [Header("Animation Management")]
    [SerializeField] AnimationController AnimationController;
    [SerializeField] AnimationStateVariable ActualAnimationState;
    [SerializeField] AnimationState FallAnimationState;
    [SerializeField] AnimationState InAirRollState;

    [Header("Camera Event")]
    [SerializeField] CameraStateEvent FallingCameraStateEvent;
    void Update()
    {
        if (ActualGravityState.Value == CoyoteHangGravity)
            return;
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
        FallingCameraStateEvent.Raise();
        if (!ActualAnimationState.Value.Equals(InAirRollState))
            AnimationController.ChangeAnimation(FallAnimationState);
    }

    private void OnDisable()
    {
        isFalling.Value = false;
    }
}