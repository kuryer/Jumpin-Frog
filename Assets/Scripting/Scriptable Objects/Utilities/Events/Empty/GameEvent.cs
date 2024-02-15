using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Utilities/Events/Parameterless/Event")]
public class GameEvent : ScriptableObject
{
    private List<EventListener> listeners
        = new List<EventListener>();

    public void Register(EventListener eventListener)
    {
        listeners.Add(eventListener);
    }

    public void Unregister(EventListener eventListener)
    {
        listeners.Remove(eventListener);
    }

    public void Raise()
    {
        foreach (var listener in listeners)
            listener.OnEventRaised();
    }
}
