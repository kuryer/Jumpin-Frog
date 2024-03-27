using UnityEngine;

public class CameraThresholdEventListener : MonoBehaviour
{
    [SerializeField] CameraThresholdEvent Event;
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

    public void Register(CameraThresholdEvent gameEvent)
    {
        if (Event != null)
            return;
        Event = gameEvent;
        Event.Register(this);
    }

    public virtual void OnEventRaised(string threshold)
    {
        StateMachine.ChangeThreshold(threshold);
    }
}
