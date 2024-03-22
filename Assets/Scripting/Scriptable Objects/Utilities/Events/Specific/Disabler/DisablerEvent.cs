using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Utilities/Events/Specific/Disabler Event")]
public class DisablerEvent : ScriptableObject
{
    List<MonoBehaviour> EnableScripts;
    List<MonoBehaviour> DisableScripts;

    public void AssignScripts(List<MonoBehaviour> enable, List<MonoBehaviour> disable)
    {
        EnableScripts = enable;
        DisableScripts = disable;
    }

    public void NullScripts()
    {
        EnableScripts = null;
        DisableScripts = null;
    }

    public void SetScripts(bool enabled)
    {
        try
        {
            if(enabled)
                foreach (var script in EnableScripts)
                    script.enabled = true;
            else
                foreach(var script in DisableScripts)
                    script.enabled = false;
        }catch(NullReferenceException)
        {
            Debug.LogError("Scripts weren't assigned to the disabler event at " + this.name);
        }
    }
}
