using UnityEngine;

public abstract class CameraState : ScriptableObject
{
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}
