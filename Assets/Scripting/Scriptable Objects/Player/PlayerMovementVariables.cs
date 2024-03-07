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
    public float swingMaxSpeed;
    public float swingAcc;
    public float swingDecc;
    public float swingPower;

    [Header("Swing Movement Redesign")]
    public float swingAcceleration;
    public float swingCatchYPosRedirection;

    [Header("Wall Grab")]
    public float wallGrabClamp;
    public float wallGravityVelocityChange;

    [Header("Wall Jump")]
    [Range(0.01f, 0.99f)]
    [Tooltip("1 means only Y Velocity, 0 means only X velocity")]
    public float directionBalance;
    public float wallJumpForce;


    [Header("Basic Jump")]
    public float JumpForce;
    [Tooltip("Y Velocity multiplied by this variable is the force that's applied to the body on jump cut")]
    public float JumpCutMultiplier;

    [Header("Swing Jump")]
    public float SwingJumpForce;
    public float SwingJumpBalance;

    [Header("Bubble Throw")]
    public float BubbleThrowForce;
    public float BubbleThrowMaxTime;
    public float XThrowModifier;
    public float YThrowModifier;
}