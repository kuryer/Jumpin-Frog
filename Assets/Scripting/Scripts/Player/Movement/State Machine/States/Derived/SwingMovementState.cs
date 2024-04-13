using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Player/Movement States/Swing Movement")]
public class SwingMovementState : MovementState
{
    [Header("Player")]
    [SerializeField] Rigidbody2DRuntimeValue rb;
    [SerializeField] FloatVariable X;
    [SerializeField] PlayerMovementVariables playerVariables;
    Transform transform;
    
    [Header("Gravity")]
    [SerializeField] GravityControllerRuntimeValue gravityController;
    [SerializeField] GravityState ActiveSwingState;
    [SerializeField] GravityState InactiveSwingState;

    [Header("Swing")]
    [SerializeField] SwingVariable ActualSwing;
    Rigidbody2D SwingRB;
    Transform SwingTransform;
    Vector2 SwingPosition;

    [Header("Rotation")]
    [SerializeField] float PlayerRotationFix;

    [Header("Disabler")]
    [SerializeField] DisablerEvent SwingMovementDisablerEvent;

    [Header("Animation Management")]
    [SerializeField] AnimationControllerRuntimeValue AnimationControllerValue;
    [SerializeField] AnimationState SwingAnimationState;
    [SerializeField] AnimationState InAirRollAnimationState;

    [Header("Swing Redesign")]
    [SerializeField] float Direction;
    public override void OnEnter()
    {
        transform = rb.Item.transform;
        GetSwingData();
        SwingMovementDisablerEvent.SetScripts(true);
        AnimationControllerValue.Item.ChangeAnimation(SwingAnimationState);
    }

    void GetSwingData()
    {
        SwingRB = ActualSwing.Value.GetComponent<Rigidbody2D>();
        SwingTransform = ActualSwing.Value.transform;
        SwingPosition = SwingTransform.position;
    }

    public override void OnUpdate()
    { 
    }

    public override void OnFixedUpdate()
    {
        SwingRotation();
        SwingingMovement();
    }

    public override void OnExit()
    {
        SwingMovementDisablerEvent.SetScripts(false);
        AnimationControllerValue.Item.ChangeAnimation(InAirRollAnimationState);
        Direction = 0;
    }


    void SwingRotation()
    {
        Vector2 lookDir = SwingRB.position - rb.Item.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        angle -= PlayerRotationFix;
        rb.Item.rotation = angle;
    }

    #region Swing Movement
    void SwingingMovement()
    {
        if(rb.Item.velocity.sqrMagnitude < playerVariables.swingMaxSpd)
            rb.Item.AddForce(playerVariables.swingAcceleration * transform.right * Direction);
    }

    public void SetSwingDirection(float direction)
    {
        Direction = direction;
    }
    #endregion
}