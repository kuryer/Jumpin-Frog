using UnityEngine;

public class MovementStateMachine : MonoBehaviour
{
    [SerializeField] MovementStateVariable ActualState;
    
    void Start()
    {
        
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
    }
}
