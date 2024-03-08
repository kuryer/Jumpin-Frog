using UnityEngine;
using UnityEngine.InputSystem;

public class SwingReleaseAction : MonoBehaviour
{
    [Header("Swing Data")]
    [SerializeField] SwingVariable ActualSwing;
    Vector2 SwingPosition;
    
    [Header("Swing Release")]
    [SerializeField] TongueRenderer TongueRenderer;
    [SerializeField] SpringJoint2D TongueJoint;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GravityController GravityController;
    [SerializeField] GravityState NormalGravity;

    [Header("Scene Management")]
    [SerializeField] MovementStateMachine StateMachine;
    [SerializeField] MovementState InAirState;

    void Update()
    {
        SwingReleaseCheck();
    }

    void SwingReleaseCheck()
    {
        if (transform.position.y > SwingPosition.y)
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
        ActualSwing.SetCanSwing(false);
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
        Debug.Log("elo");
        if (enabled)
            SwingRelease();
    }
}
