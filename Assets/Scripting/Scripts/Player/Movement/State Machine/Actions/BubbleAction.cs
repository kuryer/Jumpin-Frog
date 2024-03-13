using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleAction : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerMovementVariables playerVariables;
    Bubble activeBubble;
    Vector2 bubblePositon;

    [Header("State Managment")]
    [SerializeField] MovementStateMachine StateMachine;
    [SerializeField] MovementState BubbleState;
    [SerializeField] MovementState InAirMovementState;

    [Header("Gravity")]
    [SerializeField] GravityController GravityController;
    [SerializeField] GravityState BubbleGravity;

    [Header("Move Towards Bubble Center")]
    float ElapsedMoveTime;
    [SerializeField] float MoveTime;
    [SerializeField] float MoveSpeed;
    [SerializeField] Vector2 PositionOffset;

    [Header("Bubble Throw")]
    [SerializeField] BubbleThrowController BubbleThrow;
    [SerializeField] Buffer BubbleBuffer;
    [SerializeField] BuffersController BuffersController;
    Coroutine ThrowTimerRoutine;

    [Header("Animation Management")]
    [SerializeField] AnimationController AnimationController;
    [SerializeField] AnimationState InBubbleAnimation;
    [SerializeField] AnimationState InAirRollAnimation;
    //enabled by bubble, disabled by onExit bubbleState
    private void Update()
    {
        if (BubbleBuffer.ActivityInfo.Value())
            CallBubbleThrow(false);
    }

    public void InBubbleCall(Vector2 bubblePos, Bubble bubble)
    {
        activeBubble = bubble;
        bubblePositon = bubblePos + PositionOffset;
        rb.velocity = Vector2.zero;
        StateMachine.ChangeState(BubbleState);
        GravityController.ChangeGravity(BubbleGravity);
        AnimationController.ChangeAnimation(InBubbleAnimation);
        StartCoroutine(MoveTowardsBubbleCenter());
    }

    IEnumerator MoveTowardsBubbleCenter()
    {
        ElapsedMoveTime = MoveTime;
        while (ElapsedMoveTime > 0)
        {
            rb.position = Vector2.MoveTowards(rb.position, bubblePositon, MoveSpeed * Time.deltaTime);
            ElapsedMoveTime -= Time.deltaTime;
            yield return null;
        }
        enabled = true;
        ThrowTimerRoutine = StartCoroutine(ThrowTimer());
    }

    IEnumerator ThrowTimer()
    {
        yield return new WaitForSeconds(playerVariables.BubbleThrowMaxTime);
        CallBubbleThrow(true);
    }

    void CallBubbleThrow(bool isCoroutine)
    {
        if (!isCoroutine)
        {
            BuffersController.ResetBubbleBuffer();
            BuffersController.ResetWallJumpBuffer();
            StopCoroutine(ThrowTimerRoutine);
        }
        activeBubble.PopBubble();
        AnimationController.ChangeAnimation(InAirRollAnimation);
        StateMachine.ChangeState(InAirMovementState);
        BubbleThrow.BubbleThrow();
    }
}