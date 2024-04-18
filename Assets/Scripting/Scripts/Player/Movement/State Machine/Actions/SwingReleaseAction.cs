using UnityEngine;
using UnityEngine.InputSystem;

public class SwingReleaseAction : MonoBehaviour
{
    [SerializeField] PlayerMovementVariables playerVariables;

    [Header("Swing Data")]
    [SerializeField] SwingVariable ActualSwing;
    Vector3 SwingPosition;
    
    [Header("Swing Release")]
    [SerializeField] TongueRenderer TongueRenderer;
    [SerializeField] SpringJoint2D TongueJoint;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GravityController GravityController;
    [SerializeField] GravityState NormalGravity;
    [SerializeField] float angle;

    [Header("Scene Management")]
    [SerializeField] MovementStateMachine StateMachine;
    [SerializeField] MovementState InAirState;

    void Update()
    {
        SwingReleaseCheck();
    }

    void SwingReleaseCheck()
    {
        Vector2 dir = transform.parent.position - SwingPosition;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < playerVariables.maxLeftUpperAngle && angle > playerVariables.maxRightUpperAngle)
            SwingRelease();
    }

    public void InputSwingRelease(InputAction.CallbackContext context)
    {
        if (context.canceled && enabled)
            SwingRelease();
    }

    public void SwingRelease()
    {
        TongueJoint.enabled = false;//distjoint.enabled
        rb.rotation = 0;//rb.rotation = 0
        GravityController.ChangeGravity(NormalGravity);//gravity normal
        TongueRenderer.TurnSpriteRenderer();
        TongueRenderer.StopCalculation();//tongue renderer
        ActualSwing.Value.SetUsed();
        StateMachine.ChangeState(InAirState);//change state
    }

    #region Setup

    private void OnEnable()
    {
        SwingPosition = ActualSwing.Value.transform.position;
    }

    private void OnDisable()
    {
        SwingPosition = Vector2.zero;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled)
            SwingRelease();
    }
}
