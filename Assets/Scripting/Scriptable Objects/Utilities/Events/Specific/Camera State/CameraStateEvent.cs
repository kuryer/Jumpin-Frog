using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Utilities/Events/Specific/Camera State")]
public class CameraStateEvent : ScriptableObject
{
    private List<CameraStateEventListener> listeners
        = new List<CameraStateEventListener>();

    [SerializeField] CameraState CameraState;
    public void Register(CameraStateEventListener eventListener)
    {
        listeners.Add(eventListener);
    }

    public void Unregister(CameraStateEventListener eventListener)
    {
        listeners.Remove(eventListener);
    }

    public void Raise()
    {
        foreach (var listener in listeners)
            listener.OnEventRaised(CameraState);
    }
}