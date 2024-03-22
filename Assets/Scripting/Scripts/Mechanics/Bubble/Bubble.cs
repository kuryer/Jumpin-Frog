using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("Bubble")]
    [SerializeField] CircleCollider2D BubbleCollider;
    [SerializeField] float Cooldown;
    [SerializeField] MovementStateVariable ActualState;
    [SerializeField] BubbleActionRuntimeValue BubbleAction;

    [Header("Animation")]
    [SerializeField] Animator animator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ActualState.Value is InAirMovementState)
        {
            BubbleAction.Item.InBubbleCall(Position(), this);
            PlayAnimation("EnterBubble");
        }
    }
    Vector2 Position()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    public void PopBubble()
    {
        PlayAnimation("ExitBubble");
        StartCoroutine(BubbleCooldown());
    }

    IEnumerator BubbleCooldown()
    {
        BubbleCollider.enabled = false;
        yield return new WaitForSeconds(Cooldown);
        PlayAnimation("Idle"); //RESPAWN
        BubbleCollider.enabled = true;
    }

    void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
}
