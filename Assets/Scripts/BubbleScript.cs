using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    [SerializeField] float respawnTimer;
    Collider2D bubbleCollider;
    Animator animator;
    [SerializeField] Vector2 bubbleOffset;
    Vector2 bubblePosition;
    enum AnimationState
    {
        Idle,
        EnterBubble,
        ExitBubble
    }



    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        bubbleCollider = gameObject.GetComponent<Collider2D>();
        bubblePosition = new Vector2(transform.position.x + bubbleOffset.x, transform.position.y + bubbleOffset.y);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayAnimation(AnimationState.EnterBubble);
            //move player to the center of the bubble
            Helpers.PlayerMovement.StartAdjustingToTheBubbleCenter(bubblePosition, GetComponent<BubbleScript>());
            //count time after adjusting
            //throw player
            //play animation
        }
    }

    public void ThrowPlayer()
    {
        PlayAnimation(AnimationState.ExitBubble);
        Helpers.PlayerMovement.ThrowPlayer();
        StartCoroutine(RespawnTimer());
    }
    IEnumerator RespawnTimer()
    {
        bubbleCollider.enabled = false;
        yield return new WaitForSeconds(respawnTimer);
        bubbleCollider.enabled = true;
        PlayAnimation(AnimationState.Idle);
    }


    #region Animations

    void PlayAnimation(AnimationState state)
    {
        animator.Play(state.ToString());
    }
    #endregion
}
