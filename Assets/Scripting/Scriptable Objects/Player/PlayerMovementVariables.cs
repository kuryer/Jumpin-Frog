using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Player/Movement Variables")]
public class PlayerMovementVariables : ScriptableObject
{
    [Header("Ground Movement")]
    public float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;
    [HideInInspector] public float acc { get { return maxSpeed * acceleration; } }
    [HideInInspector] public float decc { get { return maxSpeed * decceleration; } }

    [Header("In Air Movement")]
    public float InAirMaxSpeed;
    public float InAirAcc;
    public float InAirDecc;
    public float inAirPower;

    [Header("Swing Movement")]
    public float swingMaxSpeed;
    public float swingAcc;
    public float swingDecc;
    public float swingPower;

}
