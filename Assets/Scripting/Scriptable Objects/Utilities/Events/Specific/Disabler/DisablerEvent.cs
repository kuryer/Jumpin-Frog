using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Utilities/Events/Specific/Disabler Event")]
public class DisablerEvent : ScriptableObject
{
    List<MonoBehaviour> Scripts;

    public void AssignScripts(List<MonoBehaviour> scripts)
    {
        Scripts = scripts;
    }

    public void NullScripts()
    {
        Scripts = null;
    }

    public void SetScripts(bool enabled)
    {
        try
        {
            foreach (var script in Scripts)
            {
                script.enabled = enabled;
            }

        }catch(NullReferenceException ex)
        {
            Debug.LogError("Scripts weren't assigned to the disabler event at " + this.name);
        }
    }
}
