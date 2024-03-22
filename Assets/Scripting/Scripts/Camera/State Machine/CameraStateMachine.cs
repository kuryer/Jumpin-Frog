using Cinemachine;
using UnityEngine;

public class CameraStateMachine : MonoBehaviour
{
    [Header("Player Tracking")]
    [SerializeField] Transform playerTransform;
    [SerializeField] TransformRuntimeValue trackedTransform;
    [SerializeField] TransformRuntimeValue trackerTransform;

    [Header("State Management")]
    [SerializeField] CameraState InitState;
    [SerializeField] CameraStateVariable ActualState;
    [SerializeField] Animator animator;

    private void Start()
    {
        Initialize();
    }
    void Initialize()
    {
        ActualState.Value = InitState;
        ActualState.Value.OnEnter();
        trackerTransform.SetItem(transform);
        trackedTransform.SetItem(playerTransform);
    }

    void Update()
    {
        ActualState.Value.OnUpdate();
    }

    public void ChangeState(CameraState state)
    {
        if (ActualState.Value == state)
            return;
        ActualState.Value.OnExit();
        ActualState.Value = state;
        ActualState.Value.OnEnter();
    }

    public void ChangeAnimationState(string animationName)
    {
        animator.Play(animationName);
    }

    public void CMBrainEventGrabber(ICinemachineCamera cm1, ICinemachineCamera cm2)
    {
        if(cm1 != null)Debug.Log(cm1.Name);
        if(cm2 != null)Debug.Log(cm2.Name);
    }
}