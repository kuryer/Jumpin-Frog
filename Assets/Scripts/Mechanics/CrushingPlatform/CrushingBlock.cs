using System.Collections;
using UnityEngine;

public class CrushingBlock : MonoBehaviour
{
    enum States
    {
        Idle,
        Crush,
        Respawn
    }
    States state;
    Collider2D boxCollider;
    Animator animator;

    float timeToRespawn;
    [SerializeField] Vector3 rayPosition;
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask playerLayer;


    private void Awake()
    {
        state = States.Idle;
        boxCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }
    public void SetTimeToRespawn(float time)
    {
        timeToRespawn = time;
    }
    public void Crush()
    {
        if(state == States.Idle)
        {
            ChangeAnimation("CrushingBlock_Crush");
            state = States.Crush;
        }
    }

    public void TurnOffCollider()
    {
        boxCollider.enabled = false;
    }
    public void CrushingAnimationEnd()
    {
        StartCoroutine(RespawnTimer());
    }
    void ChangeAnimation(string animationName)
    {
        animator.Play(animationName);
    }
    
    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(timeToRespawn);
        Respawn();
    }
    void Respawn()
    {
        ChangeAnimation("CrushingBlock_Respawn");
        state = States.Respawn;
    }

    public void RespawnAnimationEnd()
    {
        boxCollider.enabled = true;
        state = States.Idle;
        ChangeAnimation("CrushingBlock_Idle");

    }
}
