using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    Animator animator;
    [SerializeField] bool isSet;
    enum AnimationState
    {
        RespawnPoint_SetIdle,
        RespawnPoint_UnsetIdle,
        RespawnPoint_Set
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (isSet)
        {
            animator.Play(AnimationState.RespawnPoint_SetIdle.ToString());
            Helpers.PlayerHealth.SetRespawnPoint(gameObject, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isSet)
        {
            SetRespawn();
        }
    }

    void SetRespawn()
    {
        Helpers.PlayerHealth.SetRespawnPoint(gameObject, false);
        animator.Play(AnimationState.RespawnPoint_Set.ToString());
        isSet = true;
    }

    public void ClearPreviousBool()
    {
        isSet = false;
        animator.Play(AnimationState.RespawnPoint_UnsetIdle.ToString());
    }


    #region Animation

    public void ChangeToIdleAnimation()
    {
        animator.Play(AnimationState.RespawnPoint_SetIdle.ToString());
    }

    #endregion
}
