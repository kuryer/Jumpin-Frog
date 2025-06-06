using UnityEngine;

public class MovementStateMachine : MonoBehaviour
{
    [SerializeField] MovementStateVariable ActualState;
    [SerializeField] MovementState InitState;
    [SerializeField] BuffersController BuffersController;

    [SerializeField] MovementStateMachineRuntimeValue RuntimeValue;

    #region Runtime Value Assignment

    private void OnEnable()
    {
        RuntimeValue.SetItem(this);
    }

    private void OnDisable()
    {
        if(RuntimeValue.Item == this)
            RuntimeValue.NullItem();
    }

    #endregion

    void Start()
    {
        ActualState.Value = InitState;
    }

    void Update()
    {
        ActualState.Value.OnUpdate();
    }

    void FixedUpdate()
    {
        ActualState.Value.OnFixedUpdate();
    }

    public void ChangeState(MovementState movementState)
    {
        if (ActualState.Value == movementState)
            return;
        ActualState.Value.OnExit();
        ActualState.Value = movementState;
        ActualState.Value.OnEnter();
        BuffersController.OnStateChanged();
    }
}
