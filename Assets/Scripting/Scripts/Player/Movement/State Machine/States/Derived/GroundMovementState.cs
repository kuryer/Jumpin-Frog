using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Player/Movement States/Ground Movement")]
public class GroundMovementState : MovementState
{
    [Header("Movement")]
    [SerializeField] Rigidbody2DRuntimeValue rb;
    [SerializeField] FloatVariable X;
    [SerializeField] PlayerMovementVariables playerVariables;
    [SerializeField] BoolVariable isOnSlopeVariable;
    Rigidbody2D platformRB;
    [SerializeField] float velocity;
    bool StandOnSlope;
    
    [Header("Disabler")]
    [SerializeField] DisablerEvent GroundMovementDisablerEvent;
    
    public override void OnEnter()
    {
        velocity = rb.Item.velocity.x;
        GroundMovementDisablerEvent.SetScripts(true);
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
        //float velocityX = platformRB is null ? velocity : velocity + platformRB.velocity.x;
        //to jest do poprawy bo mam unassignedReferenceException rzucany wiêc musze ten assignment ograæ jakoœ fajnie
        rb.Item.velocity = new Vector2(velocity, rb.Item.velocity.y);
    }

    void SlowDownVelocity()
    {
        if (StandOnSlope)
            return;

        if (!StandOnSlope && isOnSlopeVariable.Value)
        {
            rb.Item.bodyType = RigidbodyType2D.Kinematic;
            rb.Item.velocity = Vector2.zero;
            StandOnSlope = true;
            return;
        }

        if (velocity == 0)
            return;

        if (Mathf.Abs(velocity + (playerVariables.decc * -Mathf.Sign(velocity) * Time.fixedDeltaTime)) > 0)
            velocity += (playerVariables.decc * -Mathf.Sign(velocity) * Time.fixedDeltaTime);
        else velocity = 0;
    }

    void SetVelocityAcc()
    {
        if (velocity == playerVariables.maxSpeed * X.Value)
            return;

        if (StandOnSlope)
            BackToDynamic();

        float multiplier = Mathf.Sign(velocity) == X.Value ? playerVariables.acc : playerVariables.decc;

        if (Mathf.Abs(velocity + (multiplier * X.Value * Time.fixedDeltaTime)) < playerVariables.maxSpeed)
            velocity += (multiplier * X.Value * Time.fixedDeltaTime);
        else velocity = playerVariables.maxSpeed * X.Value;
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