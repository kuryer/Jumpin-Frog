using UnityEngine;

public abstract class EventListener : MonoBehaviour
{
    [SerializeField] GameEvent Event;

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

    public void Register(GameEvent gameEvent)
    {
        if (Event != null)
            return;
        Event = gameEvent;
        Event.Register(this);
    }

    public abstract void OnEventRaised();
}
