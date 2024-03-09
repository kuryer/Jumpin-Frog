using UnityEngine;

public class SwingCatchAction : MonoBehaviour
{
    [Header("Swing Check")]
    [SerializeField] Buffer SwingCatchBuffer;
    [SerializeField] SwingVariable ActualSwing;
    [SerializeField] Rigidbody2D rb;

    [Header("Swing Catch")]
    [SerializeField] TongueRenderer TongueRenderer;
    [SerializeField] SpringJoint2D TongueJoint;
    [SerializeField] BuffersController BuffersController;
    [SerializeField] GravityController GravityController;
    [SerializeField] GravityState SwingGravity;
    [SerializeField] PlayerMovementVariables playerVariables;

    [Header("Swing Hang")]
    [SerializeField] float angle;
    [SerializeField] GravityState InactiveSwingGravity;

    [Header("State Management")]
    [SerializeField] MovementStateMachine StateMachine;
    [SerializeField] SwingMovementState SwingState;

    [Header("Animation")]
    [SerializeField] AnimationController animationController;

    private void Start()
    {
        ActualSwing.Value = null;
    }

    private void Update()
    {
        SwingCatchCheck();
    }

    void SwingCatchCheck()
    {
        if (ActualSwing.Value is null)
            return;
        if (SwingCatchBuffer.ActivityInfo.Value())
            SwingCatch();
    }
    
    public void SwingCatch()
    {
        Vector3 swingPos = ActualSwing.Value.transform.position;
        angle = Vector2.Angle(transform.parent.position - ActualSwing.Value.transform.position, Vector2.right);
        BuffersController.ResetSwingCatchBuffer();
        TongueJoint.connectedAnchor = swingPos;
        TongueJoint.enabled = true;
        TongueRenderer.SetSwingPointPosition(new Vector3(swingPos.x, swingPos.y, 0f));
        TongueRenderer.StartCalculation();
        TongueRenderer.TurnSpriteRenderer();

        GravityController.ChangeGravity(SwingGravity);
        SetSwingDirection();
        StateMachine.ChangeState(SwingState);
    }

    void SetSwingDirection()
    {
        float ySign = Mathf.Sign(rb.velocity.y);
        float xSign = Mathf.Sign(rb.velocity.x);
        Debug.Log(rb.velocity.x);
        float swingDist = transform.parent.position.x - ActualSwing.Value.transform.position.x;
        float redirection = ActualSwing.Value.transform.position.y - playerVariables.swingCatchYPosRedirection;
        float angle = Vector2.Angle(transform.parent.position - ActualSwing.Value.transform.position, Vector2.right);
        if(angle > playerVariables.rightHangAngle && angle < playerVariables.leftHangAngle &&
            Mathf.Abs(rb.velocity.x) < playerVariables.hangVelocityThreshold)
        {
            SwingDirectionFunctionality(0f);
            return;
        }

        if (ySign < 0f && transform.parent.position.y > redirection)
        {
            if (swingDist > 0f)
                SwingDirectionFunctionality(-1f);
            else
                SwingDirectionFunctionality(1f);
        }
        else
            SwingDirectionFunctionality(xSign);
    }

    void SwingDirectionFunctionality(float direction)
    {
        if(direction == 0)
        {
            SwingState.SetSwingDirection(0f);
            GravityController.ChangeGravity(InactiveSwingGravity);
            return;
        }
        if (direction > 0)
        {
            SwingState.SetSwingDirection(1f);
            animationController.SetSpriteFlip(false);
        }
        else
        {
            SwingState.SetSwingDirection(-1f);
            animationController.SetSpriteFlip(true);
        }
    }
}