using UnityEngine;

public abstract class MovementState : ScriptableObject
{
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnEnter();
    public abstract void OnExit();
}
