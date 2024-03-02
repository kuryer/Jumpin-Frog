using UnityEngine;

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
    bool StandOnSlope;
    
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
        //to jest do poprawy bo mam unassignedReferenceException rzucany wiêc musze ten assignment ograæ jakoœ fajnie
        rb.Item.velocity = new Vector2(velocityX, rb.Item.velocity.y);
    }

    void SlowDownVelocity()
    {
        if (StandOnSlope)
            return;

        if (ActualAnimationState.Value != IdleState)
            AnimationControllerValue.Item.ChangeAnimation(IdleState); 
        //if (!StandOnSlope && isOnSlopeVariable.Value)
        //{
        //    rb.Item.bodyType = RigidbodyType2D.Kinematic;
        //    rb.Item.velocity = Vector2.zero;
        //    velocity = 0;
        //    StandOnSlope = true;
        //    return;
        //}

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
            //Debug.Log("slow down part");
        }
        else
        {
            velocity = 0;
            //Debug.Log("zero part");
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

    #region Platform Assign



    #endregion
}