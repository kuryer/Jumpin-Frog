using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("Bubble")]
    [SerializeField] CircleCollider2D BubbleCollider;
    [SerializeField] float Cooldown;
    [SerializeField] MovementStateVariable ActualState;

    [Header("Animation")]
    [SerializeField] Animator animator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ActualState.Value is InAirMovementState)
        {
            collision.GetComponent<BubbleAction>().InBubbleCall(Position(), this);
            //animator call
        }

        //Tutaj by sie przyda³a podmianka jednak na to Bubble Action Reference 
    }
    Vector2 Position()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    public void PopBubble()
    {
        StartCoroutine(BubbleCooldown());
    }

    IEnumerator BubbleCooldown()
    {
        BubbleCollider.enabled = false;
        yield return new WaitForSeconds(Cooldown);
        BubbleCollider.enabled = true;
    }
}
