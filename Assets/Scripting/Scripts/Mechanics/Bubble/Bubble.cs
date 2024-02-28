using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] CircleCollider2D BubbleCollider;
    [SerializeField] float Cooldown;
    [SerializeField] MovementStateVariable ActualState;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ActualState.Value is InAirMovementState)
            collision.GetComponent<BubbleAction>().InBubbleCall(Position(), this);
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
