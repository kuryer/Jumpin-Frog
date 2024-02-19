using System.Collections;
using UnityEngine;

public class BubbleThrowController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerVarsSO playerVariables;

    public void BubbleThrow(Vector2 direction)
    {
        rb.AddForce(direction * playerVariables.bubbleThrowForce, ForceMode2D.Impulse);
        StartCoroutine(SlingGravityTimer());
    }

    IEnumerator SlingGravityTimer()
    {
        //switch gravity sling(bubble)
        yield return new WaitForSeconds(playerVariables.slingGravityChangeTime);
        //if(gravityState = Sling && actualState is inAir)
            //change to default gravity
    }
}