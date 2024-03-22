using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player/Movement States/Dead")]
public class DeadState : MovementState
{
    [SerializeField] DisablerEvent DeadStateDisablerEvent;
    public override void OnEnter()
    {
        DeadStateDisablerEvent.SetScripts(false);
    }
    public override void OnFixedUpdate()
    {
    }
    public override void OnUpdate()
    {
    }
    public override void OnExit()
    {
        DeadStateDisablerEvent.SetScripts(true);
    }
}
