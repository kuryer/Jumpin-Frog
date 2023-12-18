using UnityEngine;

public abstract class PlayerState : ScriptableObject
{
    [SerializeField] MovementStateMachine StateMachine;


    [HideInInspector] public virtual void OnEnterState() { }
    [HideInInspector] public virtual void OnUpdate() { }
    [HideInInspector] public virtual void OnFixedUpdate() { }
    [HideInInspector] public virtual void OnLateUpdate() { }
    [HideInInspector] public virtual void OnExitState() { }

    public virtual void RegisterStateMachine(MovementStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public bool HasStateMachineReference()
    {
        if(StateMachine == null)
            return false;
        return true;
    }
}
