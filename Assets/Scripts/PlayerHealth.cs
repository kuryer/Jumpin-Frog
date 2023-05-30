using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerMovement movementScript;
    PlayerAnimations animationScript;
    [SerializeField] GameObject respawnPointObject;
    [SerializeField] int coinsTakenPerDeath;
    Vector3 respawnPointPosition;
    float respawnSpeed; // <- JO TU MAM CUŒ TAKIEGO NI WIM CZY TEGO UZYC CZY NIE, ZEBY RAZ USTAWIAC TE PREDKOSC CZY NI
    [SerializeField] PlayerVarsSO playerVars;
    bool isDead = false;



    void Start()
    {
        animationScript = GetComponent<PlayerAnimations>();
        movementScript = GetComponent<PlayerMovement>();
        respawnSpeed = playerVars.respawnMoveTowardSpeed;
    }


    #region Kill Player

    public void KillPlayer(int typeOfDmg)
    {
        if (!isDead)
        {
            movementScript.SetupVariablesAfterDeath();
            PlayDeathAnimation(typeOfDmg);
            movementScript.SetCanMove(false);
            movementScript.enabled = false;
            Helpers.DataManagerScript.SubtractCoins(coinsTakenPerDeath);
            Debug.Log("Player Died");
            SetIsDead(true);
        }
    }
    void PlayDeathAnimation(int typeOfDmg)
    {
        switch (typeOfDmg)
        {
            case 0:
                animationScript.ChangeAnimationState("Death_Player");
                break;
            case 1:
                animationScript.ChangeAnimationState("FallDeath_Player");
                break;
        }
    }

    public void StartMovingTowardRoutine()
    {
        StartCoroutine(MoveTowardRespawn());
    }

    IEnumerator MoveTowardRespawn()
    {
        while(transform.position != respawnPointPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, respawnPointPosition, playerVars.respawnMoveTowardSpeed);
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
        SetIsDead(false);
    }

    void SetIsDead(bool state)
    {
        isDead = state;
    }

    #endregion


    #region Respawn Point

    public void SetRespawnPoint(GameObject newRespawnPoint, bool isFirst)
    {
        Vector3 newRespawnPointPos = newRespawnPoint.transform.position;
        if (newRespawnPointPos == respawnPointPosition) return;

        if (!isFirst)
        {
            ChangePreviousRespawnPoint();
        }

        respawnPointPosition = newRespawnPointPos;
        respawnPointObject = newRespawnPoint;
    }

    void ChangePreviousRespawnPoint()
    {
        if(respawnPointObject != null)
        {
            respawnPointObject.GetComponent<RespawnPoint>().ClearPreviousBool();
        }
    }

    #endregion
}
