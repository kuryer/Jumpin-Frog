using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Player/Movement States/Swing Movement")]
public class SwingMovementState : MovementState
{
    [SerializeField] Rigidbody2DRuntimeValue rb;
    [SerializeField] FloatVariable X;
    [SerializeField] PlayerMovementVariables playerVariables;
    [Header("Gravity")]
    GravityController gravityController;
    [SerializeField] GravityState ActiveSwingState;
    [SerializeField] GravityState InactiveSwingState;
    Transform transform;

    public override void OnEnter()
    {
        transform ??= rb.Item.transform;
        gravityController ??= rb.Item.GetComponent<GravityController>();
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
        SwingingMovement();
    }

    public override void OnExit()
    {
    }

    void SwingingMovement()
    {
        SetSwingGravity();

        float maxSpeed = X.Value * playerVariables.swingMaxSpeed;
        float realSpeed = rb.Item.velocity.magnitude * Mathf.Sign(rb.Item.velocity.x);
        float speedDif = maxSpeed - realSpeed;

        speedDif = Mathf.Sign(speedDif) == Mathf.Sign(maxSpeed) ? speedDif : 0f;

        float accelRate = Mathf.Sign(realSpeed) == Mathf.Sign(maxSpeed) ? playerVariables.swingAcc : playerVariables.swingDecc;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, playerVariables.swingPower) * Mathf.Sign(speedDif);

        rb.Item.AddForce(movement * transform.right);
    }

    void SetSwingGravity()
    {
        if (X.Value == 0)
            gravityController.ChangeGravity(InactiveSwingState);
        else
            gravityController.ChangeGravity(ActiveSwingState);

    }
}
