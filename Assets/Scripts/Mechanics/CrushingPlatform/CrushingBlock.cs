using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushingBlock : MonoBehaviour
{
    // Start is called before the first frame update
    enum States
    {
        Idle,
        Crush,
        Respawn
    }
    States state;
    Collider2D boxCollider;
    Animator animator;

    [SerializeField] float timeToRespawn;
    [SerializeField] Vector3 rayPosition;
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask playerLayer;

    void Start()
    {
        state = States.Idle;
        boxCollider = GetComponent<Collider2D>();
        CrushingPlatform platform = transform.parent.GetComponent<CrushingPlatform>();
        platform.OnCrush += Crush;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + rayPosition, Vector3.right, rayDistance, playerLayer);
        if (hit.collider != null && state == States.Idle) 
        {
            Crush();
        }
    }
    void Crush()
    {
        Debug.Log("test");
        //ChangeAnimation("CrushingBlock_Crush");
        //state = States.Crush;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = state == States.Idle ? Color.green : Color.red;
        Vector3 rayDistanceV3 = new Vector3(rayDistance, 0, 0);
        Gizmos.DrawLine(transform.position + rayPosition, transform.position + rayPosition + rayDistanceV3);
    }
}
