using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName ="Scriptable Objects/Player/Movement States/Ground Movement")]
public class GroundMovementState : MovementState
{
    [Header("Movement")]
    [SerializeField] Rigidbody2DRuntimeValue rb;
    [SerializeField] FloatVariable X;
    [SerializeField] PlayerMovementVariables playerVariables;
    [SerializeField] BoolVariable isOnSlopeVariable;
    [SerializeField] Rigidbody2DRuntimeValue platformRbValue;
    [SerializeField] float velocity;
    [SerializeField] bool StandOnSlope;
    
    [Header("Disabler")]
    [SerializeField] DisablerEvent GroundMovementDisablerEvent;

    [Header("Animation Management")]
    [SerializeField] AnimationControllerRuntimeValue AnimationControllerValue;
    [SerializeField] AnimationStateVariable ActualAnimationState;
    [SerializeField] AnimationState IdleState;
    [SerializeField] AnimationState RunState;

    public override void OnEnter()
    {
        velocity = rb.Item.velocity.x;
        GroundMovementDisablerEvent.SetScripts(true);
        if (X.Value == 0)
            AnimationControllerValue.Item.ChangeAnimation(IdleState);
        else
            AnimationControllerValue.Item.ChangeAnimation(RunState);
        if (isOnSlopeVariable.Value && X.Value == 0)
            SetKinematicBody();
        else if (isOnSlopeVariable.Value)
            rb.Item.velocity = new Vector2(playerVariables.maxSpeed * X.Value , rb.Item.velocity.y);
    }
    public override void OnUpdate()
    {
    }
    public override void OnFixedUpdate()
    {
        RBMovement();
    }
    public override void OnExit()
    {
        GroundMovementDisablerEvent.SetScripts(false);
        if (StandOnSlope)
            StandOnSlope = false;
    }

    #region Core Movement
    void RBMovement()
    {
        if (X.Value == 0)
            SlowDownVelocity();
        else
            SetVelocityAcc();
        if (StandOnSlope)
            return;
        float velocityX = platformRbValue.Item is null ? velocity : velocity + platformRbValue.Item.velocity.x;
        rb.Item.velocity = new Vector2(velocityX, rb.Item.velocity.y);
    }

    void SlowDownVelocity()
    {
        if (StandOnSlope)
            return;

        if (ActualAnimationState.Value != IdleState)
            AnimationControllerValue.Item.ChangeAnimation(IdleState); 

        if (velocity == 0)
            if (isOnSlopeVariable.Value)
            {
                SetKinematicBody();
                return;
            }
            else
                return;

        if (Mathf.Abs(velocity) > playerVariables.slowDownTreshold)
        {
            velocity += (playerVariables.decc * -Mathf.Sign(velocity) * Time.fixedDeltaTime);
        }
        else
        {
            velocity = 0;
            if (rb.Item.bodyType == RigidbodyType2D.Dynamic && platformRbValue.Item is null)
                SetKinematicBody();
        }
    }

    void SetVelocityAcc()
    {
        if (ActualAnimationState.Value != RunState)
            AnimationControllerValue.Item.ChangeAnimation(RunState);

        if (velocity == playerVariables.maxSpeed * X.Value)
            return;

        if (StandOnSlope)
            BackToDynamic();

        if (Mathf.Abs(velocity) > playerVariables.maxSpeed)
        {
            float multiplier = Mathf.Sign(velocity) == X.Value ? playerVariables.overDecc : playerVariables.decc;
            velocity += (-Mathf.Sign(velocity)) * multiplier * Time.fixedDeltaTime;
        }
        else
        {
            float multiplier = Mathf.Sign(velocity) == X.Value ? playerVariables.acc : playerVariables.decc;
            if (Mathf.Abs(velocity + (multiplier * X.Value * Time.fixedDeltaTime)) < playerVariables.maxSpeed)
                velocity += (multiplier * X.Value * Time.fixedDeltaTime);
            else velocity = playerVariables.maxSpeed * X.Value;
        }
    }

    public void OnDirectionChangeVelocityCheck(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        float x = context.ReadValue<Vector2>().x;
        if (x == 0)
            return;

        if (Mathf.Sign(x) != Mathf.Sign(velocity) && Mathf.Abs(rb.Item.velocity.x) < playerVariables.slowDownTreshold)
            velocity = rb.Item.velocity.x;
    }

    void SetKinematicBody()
    {
        rb.Item.bodyType = RigidbodyType2D.Kinematic;
        rb.Item.velocity = Vector2.zero;
        AnimationControllerValue.Item.ChangeAnimation(IdleState);
        velocity = 0;
        StandOnSlope = true;
    }

    void BackToDynamic()
    {
        rb.Item.bodyType = RigidbodyType2D.Dynamic;
        StandOnSlope = false;
    }
    #endregion
}