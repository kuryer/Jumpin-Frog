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
    [SerializeField] GravityState BubbleGravity;
    [SerializeField] GravityState NormalGravity;

    [Header("Throw Check")]
    [SerializeField] MovementStateVariable ActualState;

    [Header("Throw")]
    [SerializeField] Vector2Variable ThrowDirection;


    [Header("Sling Gravity Timer")]
    [SerializeField] float slingGravityChangeTime;

    public void BubbleThrow()
    {
        rb.AddForce(ThrowDirection.Value * playerVariables.BubbleThrowForce, ForceMode2D.Impulse);
        StartCoroutine(SlingGravityTimer());
    }

    IEnumerator SlingGravityTimer()
    {
        GravityController.ChangeGravity(BubbleGravity);
        yield return new WaitForSeconds(slingGravityChangeTime);
        if(ActualGravity.Value == BubbleGravity && ActualState.Value is InAirMovementState)
            GravityController.ChangeGravity(NormalGravity);
    }
}