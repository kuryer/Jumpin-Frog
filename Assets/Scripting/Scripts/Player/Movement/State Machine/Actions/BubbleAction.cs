using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleAction : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerMovementVariables playerVariables;
    Bubble activeBubble;

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

    [Header("Bubble Throw")]
    [SerializeField] BubbleThrowController BubbleThrow;
    [SerializeField] Buffer BubbleBuffer;
    Coroutine ThrowTimerRoutine;


    //enabled by bubble, disabled by onExit bubbleState
    private void Update()
    {
        if (BubbleBuffer.ActivityInfo.Value())
            CallBubbleThrow(false);
    }

    public void InBubbleCall(Vector2 bubblePos, Bubble bubble)
    {
        activeBubble = bubble;
        StateMachine.ChangeState(BubbleState);
        GravityController.ChangeGravity(BubbleGravity);
        StartCoroutine(MoveTowardsBubbleCenter(bubblePos));
    }

    IEnumerator MoveTowardsBubbleCenter(Vector2 BubblePos)
    {
        while (ElapsedMoveTime > 0)
        {
            rb.position = Vector2.MoveTowards(rb.position, BubblePos, MoveSpeed * Time.deltaTime);
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
            StopCoroutine(ThrowTimerRoutine);
        activeBubble.PopBubble();
        StateMachine.ChangeState(InAirMovementState);
        BubbleThrow.BubbleThrow();
    }
}