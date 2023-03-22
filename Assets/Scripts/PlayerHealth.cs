using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerMovement movementScript;
    PlayerAnimations animationScript;
    [SerializeField] Vector3 respawnPoint;
    float respawnSpeed; // <- JO TU MAM CUŒ TAKIEGO NI WIM CZY TEGO UZYC CZY NIE, ZEBY RAZ USTAWIAC TE PREDKOSC CZY NI
    [SerializeField] PlayerVarsSO playerVars;


    void Start()
    {
        animationScript = GetComponent<PlayerAnimations>();
        movementScript = GetComponent<PlayerMovement>();
        respawnSpeed = playerVars.respawnMoveTowardSpeed;
    }

    #region Kill Player

    public void KillPlayer()
    {
        movementScript.SetupVariablesAfterDeath();
        animationScript.ChangeAnimationState("Death_Player");
        movementScript.SetCanMove(false);
        movementScript.enabled = false;
        Debug.Log("hello");
    }

    public void StartMovingTowardRoutine()
    {
        StartCoroutine(MoveTowardRespawn());
    }

    IEnumerator MoveTowardRespawn()
    {
        while(transform.position != respawnPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, respawnPoint, playerVars.respawnMoveTowardSpeed);
            yield return null;
        }
        Respawn();
    }

    void Respawn()
    {
        animationScript.ChangeAnimationState("Respawn_Player");
    }

    public void AfterRespawnAnimation()
    {
        movementScript.enabled = true;
        movementScript.SetCanMove(true);
    }

    #endregion

    #region Respawn Point

    public void SetRespawnPoint(Vector3 newRespawnPoint)
    {
        if (newRespawnPoint == respawnPoint) return;

        respawnPoint = newRespawnPoint;
    }

    #endregion
}
