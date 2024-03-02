using UnityEngine;
using UnityEngine.Events;

public class AnimationEventListener : EventListener
{
    [SerializeField] UnityEvent Response;
    public override void OnEventRaised()
    {
        Response.Invoke();
    }
}
