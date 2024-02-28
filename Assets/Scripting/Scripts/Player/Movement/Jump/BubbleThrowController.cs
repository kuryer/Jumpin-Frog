using System.Collections;
using UnityEngine;

public class BubbleThrowController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerMovementVariables playerVariables;

    [Header("Gravity")]
    [SerializeField] GravityController GravityController;
    [SerializeField] GravityStateVariable ActualGravity;
    [SerializeField] GravityState SlingGravity;
    [SerializeField] GravityState NormalGravity;

    [Header("Throw Check")]
    [SerializeField] BuffersController BuffersController;
    [SerializeField] Buffer BubbleBuffer;
    [SerializeField] MovementStateVariable ActualState;

    [Header("Throw")]
    [SerializeField] Vector2Variable ThrowDirection;


    [Header("Sling Gravity Timer")]
    [SerializeField] float slingGravityChangeTime;

    private void Update()
    {
        BubbleCheck();
    }
    void BubbleCheck()
    {
        if (BubbleBuffer.ActivityInfo.Value() && ActualState.Value is BubbleState)
            BubbleThrow();
    }

    public void BubbleThrow()
    {
        rb.AddForce(ThrowDirection.Value * playerVariables.BubbleThrowForce, ForceMode2D.Impulse);
        StartCoroutine(SlingGravityTimer());
    }

    IEnumerator SlingGravityTimer()
    {
        GravityController.ChangeGravity(SlingGravity);
        yield return new WaitForSeconds(slingGravityChangeTime);
        if(ActualGravity.Value == SlingGravity && ActualState.Value is InAirMovementState)
            GravityController.ChangeGravity(NormalGravity);
    }
}