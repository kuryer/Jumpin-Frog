using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Player/Movement States/In Air Movement")]
public class InAirMovementState : MovementState
{
    [Header("Movement")]
    [SerializeField] Rigidbody2DRuntimeValue rb;
    [SerializeField] FloatVariable X;
    [SerializeField] PlayerMovementVariables playerVariables;
    
    [Header("Disabler")]
    [SerializeField] DisablerEvent InAirMovementDisablerEvent;

    public override void OnEnter()
    {
        InAirMovementDisablerEvent.SetScripts(true);
    }
    public override void OnUpdate()
    {
    }
    public override void OnFixedUpdate()
    {
        InAirMovement();
    }
    public override void OnExit()
    {
        InAirMovementDisablerEvent.SetScripts(false);
    }

    void InAirMovement()
    {
        //calcualte the direction we want to move in and our desired velocity
        float maxSpeed = X.Value * playerVariables.InAirMaxSpeed;
        //calculate difference between current velocity and desired velocity
        float speedDif = maxSpeed - rb.Item.velocity.x;
        //change acceleration rate depending on situation
        float accelRate = (Mathf.Abs(maxSpeed) > 0.01f) ? playerVariables.InAirAcc : playerVariables.InAirDecc;
        //checks if our player isnt stopping himself because of higher velocity than desired max speed
        float limiter = Mathf.Sign(speedDif).Equals(Mathf.Sign(maxSpeed)) ? 1f : 0f;
        //applies acceleration to speed difference, the raises to a set power so acceleration increases with higher speeds
        //finally multiplies by sign to reapply direction
        float movement = Mathf.Pow(Mathf.Abs(maxSpeed) * accelRate, playerVariables.inAirPower) * X.Value;

        movement *= limiter;
        rb.Item.AddForce(movement * Vector2.right);
    }
}
