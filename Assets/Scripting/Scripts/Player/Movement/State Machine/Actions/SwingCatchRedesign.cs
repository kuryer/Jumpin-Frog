using UnityEngine;

public class SwingCatchRedesign : MonoBehaviour
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

    [Header("State Management")]
    [SerializeField] MovementStateMachine StateMachine;
    [SerializeField] SwingMovementState SwingState;

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
        SwingState.SetSwingDirection(Mathf.Sign(rb.velocity.x));
    }
}