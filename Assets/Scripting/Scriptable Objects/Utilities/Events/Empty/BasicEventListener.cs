using UnityEngine;
using UnityEngine.Events;

public class BasicEventListener : EventListener
{
    [SerializeField] UnityEvent Response;

    public override void OnEventRaised()
    {
        Response.Invoke();
    }
}
