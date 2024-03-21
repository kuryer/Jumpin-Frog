using UnityEngine;
using UnityEngine.Events;

public class CameraStateEventListener : MonoBehaviour
{
    [SerializeField] CameraStateEvent Event;
    [SerializeField] CameraStateMachine StateMachine;
    private void OnEnable()
    {
        if (Event != null)
            Event.Register(this);
    }

    private void OnDisable()
    {
        if (Event != null)
            Event.Unregister(this);
    }

    public void Register(CameraStateEvent gameEvent)
    {
        if (Event != null)
            return;
        Event = gameEvent;
        Event.Register(this);
    }

    public void OnEventRaised(string animationName)
    {
        StateMachine.ChangeAnimationState(animationName);
    }
}
