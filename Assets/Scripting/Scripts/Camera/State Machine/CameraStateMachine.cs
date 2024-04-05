using Cinemachine;
using UnityEngine;

public class CameraStateMachine : MonoBehaviour
{

    [Header("State Management")]
    [SerializeField] CameraState InitState;
    [SerializeField] CameraStateVariable ActualState;
    [SerializeField] Animator animator;
    [SerializeField] string Threshold;
    private void Start()
    {
        Initialize();
    }
    void Initialize()
    {
        ActualState.Value = InitState;
        ChangeThreshold("M");
        ChangeAnimationState(ActualState.Value.CameraAnimationState);
    }

    public void ChangeState(CameraState state)
    {
        if (ActualState.Value == state)
            return;
        ActualState.Value = state;
        ChangeAnimationState(ActualState.Value.CameraAnimationState);
    }

    public void ChangeThreshold(string threshold)
    {
        if (Threshold.Equals(threshold))
            return;
        Threshold = threshold;
        ChangeAnimationState(ActualState.Value.CameraAnimationState);
    }

    void ChangeAnimationState(string animationName)
    {
        animator.Play(animationName + Threshold);
    }
}