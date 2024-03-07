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

    [Header("State Management")]
    [SerializeField] MovementStateMachine StateMachine;
    [SerializeField] SwingMovementState SwingState;

    [Header("Animation")]
    [SerializeField] AnimationController animationController;

    private void Update()
    {
        SwingCatchCheck();
    }

    void SwingCatchCheck()
    {
        if (ActualSwing.Value is null)
            return;
        if (SwingCatchBuffer.ActivityInfo.Value() && transform.position.y < ActualSwing.Value.transform.position.y)
            SwingCatch();
    }
    
    public void SwingCatch()
    {
        Vector3 swingPos = ActualSwing.Value.transform.position;
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

        float swingDist = transform.parent.position.x - ActualSwing.Value.transform.position.x;
        float redirection = ActualSwing.Value.transform.position.y - playerVariables.swingCatchYPosRedirection;


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
        if(direction > 0)
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