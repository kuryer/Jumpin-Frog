using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerHealth : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] List<Collider2D> playerColliders;
    [SerializeField] Rigidbody2D rb;

    [Header("Death Sequence")]
    [SerializeField] float deathWaitTime;
    [SerializeField] float teleportWaitTime;
    [SerializeField] float respawnWaitTime;

    [Header("Gravity")]
    [SerializeField] GravityController GravityController;
    [SerializeField] GravityState ZeroGravity;
    [SerializeField] GravityState NormalGravity;

    [Header("State Management")]
    [SerializeField] MovementStateMachine StateMachine;
    [SerializeField] MovementState DeathState;

    [Header("Respawn")]
    [SerializeField] MovementState GroundMovementState;
    [SerializeField] RespawnPointVariables RespawnInfo;
    [SerializeField] BoolVariable isInRespawnAnimation;
    [SerializeField] float respawnAnimationDuration;
    [SerializeField] GameEvent OnRespawnEvent;

    [Header("Animation Management")]
    [SerializeField] AnimationController animationController;
    [SerializeField] AnimationState DeathAnimation;
    [SerializeField] AnimationState RespawnAnimation;

    private void Start()
    {
        if (RespawnInfo.ActualRespawnPoint is not NewRespawnPoint)
            RespawnInfo.SetPosition(transform.position);
    }

    private void OnDisable()
    {
        RespawnInfo.ResetPoint();
    }

    public void PlayerDeath()
    {
        animationController.ChangeAnimation(DeathAnimation);
        SetColliders(false);
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;
        GravityController.ChangeGravity(ZeroGravity);
        StateMachine.ChangeState(DeathState);
        StartCoroutine(RespawnTeleport());
    }
    [SerializeField] GameEvent OnDeathTeleportEvent;
    IEnumerator RespawnTeleport()
    {
        float timeElapsed = 0f;
        Vector3 cachedDeathPosition = transform.position;
        Vector3 resp = RespawnInfo.GetPosition();
        yield return new WaitForSeconds(deathWaitTime);
        OnDeathTeleportEvent.Raise();
        while (timeElapsed <= teleportWaitTime)
        {
            timeElapsed += Time.deltaTime;
            transform.parent.position = Vector3.Lerp(cachedDeathPosition, resp, timeElapsed / teleportWaitTime);
            yield return null;
        }
        yield return new WaitForSeconds(respawnWaitTime);
        Respawn();
    }

    void Respawn()
    {
        SetColliders(true);
        StateMachine.ChangeState(GroundMovementState);
        GravityController.ChangeGravity(NormalGravity);
        rb.bodyType = RigidbodyType2D.Dynamic;
        animationController.ChangeAnimation(RespawnAnimation);
        isInRespawnAnimation.Value = true;
        StartCoroutine(RespawnAnimationHandle());
    }

    IEnumerator RespawnAnimationHandle()
    {
        yield return new WaitForSeconds(respawnAnimationDuration);
        OnRespawnEvent.Raise();
        isInRespawnAnimation.Value = false;
    }

    void SetColliders(bool areActive)
    {
        foreach(Collider2D coll in playerColliders)
            coll.enabled = areActive;
    }

}
