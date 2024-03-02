using System.Collections;
using UnityEngine;

public class NewPlayerHealth : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Collider2D playerCollider;
    [SerializeField] Rigidbody2D rb;

    [Header("Death Sequence")]
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
        //call animation
        playerCollider.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;
        GravityController.ChangeGravity(ZeroGravity);
        StateMachine.ChangeState(DeathState);
        StartCoroutine(RespawnTeleport());
    }

    IEnumerator RespawnTeleport()
    {
        yield return new WaitForSeconds(teleportWaitTime);
        TeleportToRespawnPoint();
        yield return new WaitForSeconds(respawnWaitTime);
        Respawn();
    }

    void TeleportToRespawnPoint()
    {
        Vector2 resp = RespawnInfo.GetPosition();
        transform.parent.position = new Vector3(resp.x, resp.y, transform.parent.position.z);
    }
    void Respawn()
    {
        playerCollider.enabled = true;
        StateMachine.ChangeState(GroundMovementState);
        GravityController.ChangeGravity(NormalGravity);
        rb.bodyType = RigidbodyType2D.Dynamic;
        //call animation
    }
}
