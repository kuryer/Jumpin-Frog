using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Player/Movement Variables")]
public class PlayerMovementVariables : ScriptableObject
{
    [Header("Ground Movement")]
    public float maxSpeed;
    public float slowDownTreshold;
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;
    [Tooltip("Force that will be applied to slow down the player when its velocity will be higher than the maximum possible on ground Speed")]
    [SerializeField] float overspeedDecceleration;

    [HideInInspector] public float overDecc { get { return maxSpeed * overspeedDecceleration; } }
    [HideInInspector] public float acc { get { return maxSpeed * acceleration; } }
    [HideInInspector] public float decc { get { return maxSpeed * decceleration; } }

    [Header("In Air Movement")]
    public float InAirMaxSpeed;
    public float InAirAcc;
    public float InAirDecc;
    public float inAirPower;

    public float FallClampSpeed;

    [Header("Swing Movement")]
    [SerializeField] float swingMaxSpeed;
    [HideInInspector] public float swingMaxSpd { get { return swingMaxSpeed * swingMaxSpeed; } }
    public float swingAcceleration;
    public float swingCatchYPosRedirection;

    [Header("Wall Glide")]
    public float slowValue;

    [Header("Wall Grab")]
    public float wallGrabClamp;
    public float wallGravityVelocityChange;
    public float wallGrabXForce;

    [Header("Wall Jump")]
    [Range(0.01f, 0.99f)]
    [Tooltip("1 means only Y Velocity, 0 means only X velocity")]
    public float directionBalance;
    public float wallJumpForce;
    public float wallHangVelocity;

    [Header("Basic Jump")]
    public float JumpForce;
    [Tooltip("Y Velocity multiplied by this variable is the force that's applied to the body on jump cut")]
    public float JumpCutMultiplier;

    [Header("Swing Jump")]
    public float SwingJumpForce;
    public float SwingJumpBalance;

    [Header("Swing Jump Detection")]
    [Tooltip("If players velocity is higher than this variable the player will be able to swing jump")]
    [SerializeField] float ThresholdVelocity;
    [HideInInspector]public float thresholdVel { get { return ThresholdVelocity * ThresholdVelocity; } }
    public float DetectionAreaAngle;
    public float upperDetectionAreaAngle;
    [HideInInspector] public float maxLeftAngle { get { return 180f - DetectionAreaAngle; } }
    [HideInInspector] public float maxRightAngle { get { return DetectionAreaAngle; } }
    [HideInInspector] public float maxLeftUpperAngle { get { return 180 - upperDetectionAreaAngle; } }
    [HideInInspector] public float maxRightUpperAngle { get { return upperDetectionAreaAngle; } }

    public bool IsInRightSwingJumpArea(float angle)
    {
        return angle < maxRightUpperAngle && angle > -maxRightAngle;
    }

    public bool IsInLeftSwingJumpArea(float angle)
    {
        return angle > maxLeftUpperAngle || angle < -maxLeftAngle;
    }

    [Header("Swing Hang")]
    [SerializeField] float hangAngle;
    [HideInInspector] public float leftHangAngle { get { return 90 + hangAngle; } }
    [HideInInspector] public float rightHangAngle { get { return 90 - hangAngle; } }
    public float swingHangBar;
    public float hangVelocityThreshold;

    [Header("Swing Debug")]
    public bool showSwingJumpArea;
    public Color inSwingJumpAreaColor;
    public bool showSwingHangArea;
    public bool showRedirectionBar;

    [Header("Bubble Throw")]
    public float BubbleThrowForce;
    public float BubbleThrowMaxTime;
    public float XThrowModifier;
    public float YThrowModifier;
}