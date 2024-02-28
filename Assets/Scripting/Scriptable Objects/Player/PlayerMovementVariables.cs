using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Player/Movement Variables")]
public class PlayerMovementVariables : ScriptableObject
{
    [Header("Ground Movement")]
    public float maxSpeed;
    public float slowDownTreshold;
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;
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

    [Header("Basic Jump")]
    public float JumpForce;
    [Tooltip("Y Velocity multiplied by this variable is the force that's applied to the body on jump cut")]
    public float JumpCutMultiplier;

    [Header("Swing Jump")]
    public float SwingJumpForce;
    public float SwingJumpBalance;

}
