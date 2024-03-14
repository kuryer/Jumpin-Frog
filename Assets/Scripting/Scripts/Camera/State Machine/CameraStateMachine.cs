using System.Collections;
using System.Collections.Generic;
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


    void LateUpdate()
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
}
