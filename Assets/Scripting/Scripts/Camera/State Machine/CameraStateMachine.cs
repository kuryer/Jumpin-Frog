using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraStateMachine : MonoBehaviour
{
    [Header("Player Tracking")]
    [SerializeField] Transform playerTransform;
    [SerializeField] TransformRuntimeValue trackedTransform;
    [SerializeField] TransformRuntimeValue trackerTransform;

    [Header("State Management")]
    [SerializeField] CameraState InitState;
    [SerializeField] CameraStateVariable ActualState;

    [Header("Debug")]
    [SerializeField] InputAction HeightAction;
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

    public void AddHeight()
    {
        transform.position += Vector3.up * 2;
    }
}
