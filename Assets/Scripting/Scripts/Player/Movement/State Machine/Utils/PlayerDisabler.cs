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

    [Header("Swing Movement")]
    [SerializeField] DisablerEvent SwingMovementDisablerEvent;
    [SerializeField] List<MonoBehaviour> SwingEnableScripts;
    [SerializeField] List<MonoBehaviour> SwingDisableScripts;

    #region Assignment

    private void OnEnable()
    {
        AssignGroundMovement(true);
        AssignInAirMovement(true);
        AssignSwingMovement(true);
    }

    private void OnDisable()
    {
        AssignGroundMovement(false);
        AssignInAirMovement(false);
        AssignSwingMovement(false);
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

    void AssignSwingMovement(bool enabled)
    {
        if (enabled)
            SwingMovementDisablerEvent.AssignScripts(SwingEnableScripts, SwingDisableScripts);
        else
            SwingMovementDisablerEvent.NullScripts();
    }
}