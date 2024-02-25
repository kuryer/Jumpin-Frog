using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisabler : MonoBehaviour
{
    [Header("Ground Movement")]
    [SerializeField] DisablerEvent GroundMovementStateEvent;
    [SerializeField] List<MonoBehaviour> GroundScripts;

    [Header("In Air Movement")]
    [SerializeField] DisablerEvent InAirMovementStateEvent;
    [SerializeField] List<MonoBehaviour> InAirScripts;


    #region Assignment

    private void OnEnable()
    {
        AssignGroundMovement(true);
        AssignInAirMovement(true);
    }

    private void OnDisable()
    {
        AssignGroundMovement(false);
        AssignInAirMovement(false);
    }

    #endregion


    void AssignGroundMovement(bool enabled)
    {
        if(enabled)
            GroundMovementStateEvent.AssignScripts(GroundScripts);
        else
            GroundMovementStateEvent.NullScripts();
    }

    void AssignInAirMovement(bool enabled)
    {
        if (enabled)
            InAirMovementStateEvent.AssignScripts(InAirScripts);
        else
            InAirMovementStateEvent.NullScripts();
    }
}