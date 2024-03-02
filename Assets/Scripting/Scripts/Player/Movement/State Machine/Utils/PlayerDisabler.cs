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

    [Header("On Wall Movement")]
    [SerializeField] DisablerEvent OnWallDisablerEvent;
    [SerializeField] List<MonoBehaviour> OnWallEnableScripts;
    [SerializeField] List<MonoBehaviour> OnWallDisableScripts;

    [Header("Bubble State")]
    [SerializeField] DisablerEvent BubbleDisablerEvent;
    [SerializeField] List<MonoBehaviour> BubbleEnableScripts;
    [SerializeField] List<MonoBehaviour> BubbleDisableScripts;

    [Header("Dead State")]
    [SerializeField] DisablerEvent DeadStateDisablerEvent;
    [SerializeField] List<MonoBehaviour> DeadStateEnableScripts;
    [SerializeField] List<MonoBehaviour> DeadStateDisableScripts;

    #region Assignment

    private void OnEnable()
    {
        AssignGroundMovement(true);
        AssignInAirMovement(true);
        AssignSwingMovement(true);
        AssignOnWallMovement(true);
        AssignBubbleMovement(true);
        AssignDeadMovement(true);
    }

    private void OnDisable()
    {
        AssignGroundMovement(false);
        AssignInAirMovement(false);
        AssignSwingMovement(false);
        AssignOnWallMovement(false);
        AssignBubbleMovement(false);
        AssignDeadMovement(false);
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

    void AssignOnWallMovement(bool enabled)
    {
        if (enabled)
            OnWallDisablerEvent.AssignScripts(OnWallEnableScripts, OnWallDisableScripts);
        else
            OnWallDisablerEvent.NullScripts();
    }

    void AssignBubbleMovement(bool enabled)
    {
        if (enabled)
            BubbleDisablerEvent.AssignScripts(BubbleEnableScripts, BubbleDisableScripts);
        else
            BubbleDisablerEvent.NullScripts();
    }

    void AssignDeadMovement(bool enabled)
    {
        if (enabled)
            DeadStateDisablerEvent.AssignScripts(DeadStateEnableScripts, DeadStateDisableScripts);
        else
            DeadStateDisablerEvent.NullScripts();
    }
}