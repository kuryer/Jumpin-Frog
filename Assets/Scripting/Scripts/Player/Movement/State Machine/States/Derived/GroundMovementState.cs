using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Player/Movement States/Ground Movement")]
public class GroundMovementState : MovementState
{
    [Header("Movement")]
    [SerializeField] Rigidbody2DRuntimeValue rb;
    [SerializeField] IntVariable X;
    [SerializeField] PlayerVarsSO playerVariables;
    [SerializeField] FloatVariable SlopeModifier;
    public override void OnEnter()
    {
    }
    public override void OnUpdate()
    {
    }
    public override void OnFixedUpdate()
    {
        float maxSpeed = X.Value * playerVariables.moveSpeed * SlopeModifier.Value;

        float baseVelocity = rb.Item.velocity.x;

        /*if (platformRB != null)
        {
            float platfromNormalizedVelocityX = platformRB.velocity.x * playerVars.naturalizerModifier;
            baseVelocity -= platfromNormalizedVelocityX;
        }*/
        //^^^ to jest od platform movementu to bedzie trzeba przebadaæ
        float speedDif = maxSpeed - baseVelocity;

        float accelRate = (Mathf.Abs(maxSpeed) > 0.01f) ? playerVariables.acceleration : playerVariables.decceleration;

        float movement = (Mathf.Abs(speedDif) * accelRate) * Mathf.Sign(speedDif);

        rb.Item.AddForce(movement * Vector2.right);
    }
    public override void OnExit()
    {
    }
}