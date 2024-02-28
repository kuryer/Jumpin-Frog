using UnityEngine;

public class SwingCatchAction : MonoBehaviour
{
    [Header("Swing Check")]
    [SerializeField] Buffer SwingCatchBuffer;
    [SerializeField] SwingVariable ActualSwing;

    [Header("Swing Catch")]
    [SerializeField] TongueRenderer TongueRenderer;
    [SerializeField] SpringJoint2D TongueJoint;
    [SerializeField] BuffersController BuffersController;
    [SerializeField] GravityController GravityController;
    [SerializeField] GravityState SwingGravity;

    [Header("Scene Management")]
    [SerializeField] MovementStateMachine StateMachine;
    [SerializeField] MovementState SwingState;

    private void Update()
    {
        SwingCatchCheck();
    }

    void SwingCatchCheck()
    {
        if (SwingCatchBuffer.ActivityInfo.Value() && ActualSwing.Value != null)
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
        StateMachine.ChangeState(SwingState);
    }
}
