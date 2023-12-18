using UnityEngine;

public class MovementStateMachine : MonoBehaviour
{

    [SerializeField] PlayerState starterMovementState;
    [SerializeField] PlayerState starterJumpState;

    private PlayerState MovementState;
    private PlayerState JumpState;

    [Header("Movement")]
    public float X;

    PlayerControls playerInput;
    private void Awake()
    {
        //playerInput = new PlayerControls();
    }

    private void OnEnable()
    {
        //ChangeState(starterMovementState);
        //ChangeState(starterJumpState);
    }

    private void Update()
    {
        GatherInput();
    }

    #region Input

    void GatherInput()
    {
        X = Input.GetAxisRaw("Horizontal");
    }

    #endregion

    #region Change State
    public void ChangeState(PlayerState newState)
    {
        if (!newState.HasStateMachineReference())
            newState.RegisterStateMachine(this);

        if(newState is MovementState)
            ChangeMovementState(newState);
        else if(newState is JumpState)
            ChangeJumpState(newState);
    }
    void ChangeMovementState(PlayerState movementState)
    {
        MovementState.OnExitState();
        MovementState = movementState;
        MovementState.OnEnterState();
    }

    void ChangeJumpState(PlayerState jumpState)
    {
        JumpState.OnExitState();
        JumpState = jumpState;
        JumpState.OnEnterState();
    }
    #endregion
}
