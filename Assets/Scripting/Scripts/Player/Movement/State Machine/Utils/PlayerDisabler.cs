using System.Collections.Generic;
using UnityEngine;

public class PlayerDisabler : MonoBehaviour
{
    [Header("Ground Movement")]
    [SerializeField] DisablerEvent GroundMovementDisablerEvent;
    [SerializeField] List<MonoBehaviour> GroundEnableScripts;
    [SerializeField] List<MonoBehaviour> GroundDisableScripts;

    [Header("In Air Movement")]
    [SerializeField] DisablerEvent InAirMovementDisablerEvent;
    [SerializeField] List<MonoBehaviour> InAirEnableScripts;
    [SerializeField] List<MonoBehaviour> InAirDisableScripts;


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
            GroundMovementDisablerEvent.AssignScripts(GroundEnableScripts, GroundDisableScripts);
        else
            GroundMovementDisablerEvent.NullScripts();
    }

    void AssignInAirMovement(bool enabled)
    {
        if (enabled)
            InAirMovementDisablerEvent.AssignScripts(InAirEnableScripts, InAirDisableScripts);
        else
            InAirMovementDisablerEvent.NullScripts();
    }
}