using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Utilities/Events/Specific/Camera Threshold")]
public class CameraThresholdEvent : ScriptableObject
{
    private List<CameraThresholdEventListener> listeners 
        = new List<CameraThresholdEventListener>();

    [SerializeField] string Threshold;

    public void Register(CameraThresholdEventListener eventListener)
    {
        listeners.Add(eventListener);
    }

    public void Unregister(CameraThresholdEventListener eventListener)
    {
        listeners.Remove(eventListener);
    }

    public void Raise()
    {
        foreach (var listener in listeners)
            listener.OnEventRaised(Threshold);
    }
}
